using alumni.Data;
using alumni.Domain;
using alumni.Hubs;
using alumni.IServices;
using alumni.QueueProcessing;
using Alumni.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class VideoService : IVideoService
    {
        private const string WORKING_DIRECTORY = "ffmpeg";
        private const string CONV_EXE = "conv.bat";
        private const string FFPROBE_EXE = "ffprobe.exe";
        private const string FFMPEG_EXE = "ffmpeg.exe";
        private const string TEMPORARY_PREFIX = "temp_";
        private const string COMPLETED_PREFIX = "comp_";
        private const string VIDEO_COMPLETED_PATH = "videos\\comp";
        private const string VIDEO_TEMPORARY_PATH = "videos\\temp";
        private const string VIDEO_THUMBNAIL_PATH = "videos\\thumbnail";
        private const string SHORT_VIDEO_PATH = "videos\\shortvideo";
        private const int SHORT_VIDEO_DURATION = 10;
        private const int SHORT_VIDEO_START = 0;
        private const string DASH_MANIFEST_NAME = "dash.mpd";

        private readonly DataContext dataContext;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<VideoService> logger;
        private readonly IQueueBackgroundServices queueBackground;

        public VideoService(DataContext dataContext,
            ILogger<VideoService> logger,
            IWebHostEnvironment env,
            IQueueBackgroundServices queueBackground)
        {
            this.dataContext = dataContext;

            this.logger = logger;

            this.env = env;

            this.queueBackground = queueBackground;            
        }

        public async Task<Video> CreateTemporaryVideoAsync(IFormFile file)
        {
            var temporaryVideoName = GetTemporaryVideoName(file.FileName);
            var temporaryVideoDir = GetTemporaryVideoDir(temporaryVideoName);
            CreateTemporaryVideoDir(temporaryVideoName);

            if (temporaryVideoDir == null) return null;

            try
            {
                using (var fileStream = new FileStream(Path.Combine(temporaryVideoDir, temporaryVideoName), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                var video = new Video
                {
                    Id = Guid.NewGuid().ToString(),
                    CrationAt = DateTime.UtcNow,
                    TempFileName = temporaryVideoName,
                    Converted = false
                };

                await dataContext.Videos.AddAsync(video);

                var stt = await dataContext.SaveChangesAsync();

                if (stt < 1)
                {
                    DeleteDir(temporaryVideoDir);

                    logger.LogError("Cannot save video in database");

                    return null;
                }

                return video;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot create video in {0}", temporaryVideoDir);
                return null;
            }
        }

        // This will be executed in background level
        public void GetVideoDurationAsync(string videoId, string connectionId)
        {
            queueBackground.QueueTask(async (token, serviceProvider) =>
            {

                if (token.IsCancellationRequested) return;

                using (var scope = serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var hub = serviceProvider.GetRequiredService<IHubContext<VideoUploadHub>>();

                    var video = await dataContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);

                    if (video == null) return;

                    var videoDir = GetTemporaryVideoDir(video.TempFileName);

                    var info = new ProcessStartInfo
                    {
                        FileName = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY, FFPROBE_EXE),
                        WorkingDirectory = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY),
                        Arguments = $"-v error -show_entries format=duration -of default=nw=1:nk=1 -sexagesimal {Path.Combine(videoDir, video.TempFileName)}",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    };

                    using (var process = new Process { StartInfo = info })
                    {
                        process.Start();

                        var stdErr = process.StandardError;
                        var stdOut = process.StandardOutput;

                        process.WaitForExit();

                        var duration = await stdOut.ReadToEndAsync();

                        try
                        {
                            var timeSpan = TimeSpan.Parse(duration);

                            video.Duration = timeSpan.ToString(@"hh\:mm\:ss");

                            dataContext.Entry<Video>(video).State = EntityState.Modified;

                            var result = await dataContext.SaveChangesAsync();

                            if (result > 1)
                            {
                                await hub.Clients.Client(connectionId).SendAsync("receiveVideoInfo", "Video duration supplied");
                            }//TODO: If not updated do thing man
                        }
                        catch (Exception e)
                        {
                            logger.LogError($"Cannot get the video duration of {0} file", video.TempFileName);

                            DeleteDir(videoDir);

                            //TODO: If not time span parse, do thing man
                        }
                    }
                }
            });
        }

        // This will be executed in background level
        public void GetFrameCountAsync(string videoId, string connectionId)
        {
            queueBackground.QueueTask(async (token, serviceProvider) =>
            {

                if (token.IsCancellationRequested) return;

                using (var scope = serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var hub = serviceProvider.GetRequiredService<IHubContext<VideoUploadHub>>();

                    var video = await dataContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);

                    if (video == null) return;

                    var videoDir = GetTemporaryVideoDir(video.TempFileName);

                    var info = new ProcessStartInfo
                    {
                        FileName = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY, FFPROBE_EXE),
                        WorkingDirectory = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY),
                        Arguments = $"-v error -count_frames -select_streams v:0 -show_entries stream=nb_read_frames -of default=nw=1:nk=1 {Path.Combine(videoDir, video.TempFileName)}",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    };

                    using (var process = new Process { StartInfo = info })
                    {
                        process.Start();

                        var stdErr = process.StandardError;
                        var stdOut = process.StandardOutput;

                        process.WaitForExit();

                        var frameCount = await stdOut.ReadToEndAsync();

                        try
                        {
                            video.FrameLength = int.Parse(frameCount);

                            var errorTx = Math.Round(video.FrameLength * 15.0 / 100);

                            video.FrameLength -= (int)errorTx;

                            dataContext.Entry<Video>(video).State = EntityState.Modified;

                            var result = await dataContext.SaveChangesAsync();

                            if (result > 1)
                            {
                                await hub.Clients.Client(connectionId).SendAsync("receiveVideoInfo", "Video frame count supplied");
                            }
                            //TODO: If not updated, do thing man
                        }
                        catch (Exception e)
                        {
                            logger.LogError($"Cannot get the video frame count of {0} file", video.TempFileName);

                            DeleteDir(videoDir);

                            //TODO: If not converted do thing man
                        }
                    }
                }
            });
        }

        // This will be executed in background level
        public void GetShortVideoAsync(string videoId)
        {
            queueBackground.QueueTask(async (token, serviceProvider) =>
            {
                if (token.IsCancellationRequested) return;

                using (var scope = serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var video = await dataContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);

                    if (video == null) return;

                    var videoDir = GetTemporaryVideoDir(video.TempFileName);
                    var shortVideoName = GetShortVideoName(video.TempFileName);
                    CreateShortVideoDir(shortVideoName);
                    var shortVideoDir = GetShortVideoDir(shortVideoName);

                    var startingAt = SHORT_VIDEO_START;
                    var duration = SHORT_VIDEO_DURATION;

                    if (TimeSpan.Parse(video.Duration).Seconds < duration)
                    {
                        duration = (int)Math.Round((double) TimeSpan.Parse(video.Duration).Seconds);
                    }

                    var info = new ProcessStartInfo
                    {
                        FileName = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY, FFMPEG_EXE),
                        WorkingDirectory = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY),
                        Arguments = $"-ss 00:00:{startingAt} -i {Path.Combine(videoDir, video.TempFileName)} -t 00:00:{duration} -c copy -an {Path.Combine(shortVideoDir, shortVideoName)}",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    };

                    using (var process = new Process { StartInfo = info })
                    {
                        try
                        {
                            process.Start();

                            process.WaitForExit();
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e.Message);
                        }
                    }

                    try
                    {
                        video.ShortVideoPath = GetPublicPath(Path.Combine(shortVideoDir, shortVideoName));

                        dataContext.Entry<Video>(video).State = EntityState.Modified;

                        var result = await dataContext.SaveChangesAsync();
                    }
                    catch (DbException e)
                    {
                        logger.LogError(e.Message);
                    }
                }
            });
        }

        // This will be executed in background level
        public void GetThumbnailVideoAsync(string videoId, string connectionId)
        {
            queueBackground.QueueTask(async (token, serviceProvider) =>
            {

                if (token.IsCancellationRequested) return;

                using (var scope = serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<VideoUploadHub>>();

                    var video = await dataContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);

                    if (video == null) return;

                    var videoDir = GetTemporaryVideoDir(video.TempFileName);
                    var thumbnailName = GetThumbnailName(video.TempFileName);
                    CreateThumbnailDir(thumbnailName);
                    var thumbnailDir = GetThumbnailDir(thumbnailName);
                    thumbnailName += ".jpg";

                    var info = new ProcessStartInfo
                    {
                        FileName = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY, FFMPEG_EXE),
                        WorkingDirectory = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY),
                        Arguments = $"-ss 00:00:01 -i {Path.Combine(videoDir, video.TempFileName)} -vframes 1 -q:v 2 {Path.Combine(thumbnailDir, thumbnailName)}",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    };

                    using (var process = new Process { StartInfo = info })
                    {
                        try
                        {
                            process.Start();

                            process.WaitForExit();
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e.Message);
                        }
                    }

                    try
                    {
                        video.ThumbnailPath = GetPublicPath(Path.Combine(thumbnailDir, thumbnailName));

                        dataContext.Entry<Video>(video).State = EntityState.Modified;

                        var result = await dataContext.SaveChangesAsync();

                        if (result >= 1)
                        {
                            await hub.Clients.Client(connectionId).SendAsync("receiveThumbnail", video.ThumbnailPath);
                        }

                    }
                    catch (DbException e)
                    {
                        logger.LogError(e.Message);
                    }
                }
            });
        }

        // This will be executed in background level
        public void ExecuteDashConversionAsync(string videoId, string connectionId)
        {
            queueBackground.QueueTask(async (token, serviceProvider) =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var hub = serviceProvider.GetRequiredService<IHubContext<VideoUploadHub>>();

                    var video = await dataContext.Videos.FirstOrDefaultAsync(v => v.Id == videoId);

                    if (video == null) return;

                    var videoTemporaryDir = GetTemporaryVideoDir(video.TempFileName);
                    var videoCompletedDir = GetCompletedVideoDir(GetCompletedVideoName(video.TempFileName));
                    CreateCompletedVideoDir(videoCompletedDir);

                    var info = new ProcessStartInfo
                    {
                        WorkingDirectory = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY),
                        FileName = Path.Combine(env.ContentRootPath, WORKING_DIRECTORY, CONV_EXE),
                        Arguments = $"{Path.Combine(videoTemporaryDir, video.TempFileName)} {videoCompletedDir}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                    };

                    using (var process = new Process { StartInfo = info })
                    {
                        logger.LogInformation("Video convertion background starting");

                        try
                        {
                            process.Start();
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e.Message);
                        }

                        var stdErr = process.StandardError;

                        do
                        {
                            if (token.IsCancellationRequested) return;

                            var conversionInfo = await stdErr.ReadLineAsync();

                            var percentage = 0.0;

                            if (conversionInfo != null && conversionInfo.Contains("frame="))
                            {
                                var lineHandlered = conversionInfo.Substring(conversionInfo.IndexOf("frame=") + ("frame=").Length).TrimStart()
                                .TakeWhile((value) => int.TryParse(value.ToString(), out _));

                                try
                                {
                                    int.TryParse(new string(lineHandlered.ToArray()), out int frame);

                                    float result = (100 * frame) / video.FrameLength;

                                    percentage = Math.Round(result);

                                    logger.LogInformation($"Frame = {frame}");
                                }
                                catch
                                {
                                    //TODO: Handler some convertion problem
                                }

                            }

                            Console.WriteLine($"Percentage: {percentage}");

                            await hub.Clients.Client(connectionId).SendAsync("conversionProgress", percentage);

                        } while (!process.HasExited);
                    }

                    try
                    {
                        video.Converted = true;                        
                        video.ManifestPath = Path.Combine(GetPublicPath(videoCompletedDir), DASH_MANIFEST_NAME);

                        dataContext.Entry(video).State = EntityState.Modified;

                        var stt = await dataContext.SaveChangesAsync();

                        if (stt >= 0)
                        {
                            await hub.Clients.Client(connectionId).SendAsync("receiveManifest", video.ManifestPath);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError("Cannot save the converted video");
                    }
                    finally
                    {
                        logger.LogInformation("Video convertion in background mode is finish");

                        DeleteDir(videoTemporaryDir);
                    }
                }
            });
        }        

        private string GetTemporaryVideoName(string fileName)
        {
            var mime = GetMime(fileName);

            var tempFileName = $"{TEMPORARY_PREFIX}{Path.GetRandomFileName()}";

            var filePath = string.Concat(tempFileName, '.', mime);

            return filePath;
        }

        private string GetCompletedVideoName(string fileName)
        {
            return GetFileNameWithNoExtension(fileName.Substring(TEMPORARY_PREFIX.Length).Insert(0, COMPLETED_PREFIX));
        }

        private string GetShortVideoName(string fileName)
        {
            return fileName.Substring(TEMPORARY_PREFIX.Length);
        }

        private string GetThumbnailName(string fileName)
        {
            return GetFileNameWithNoExtension(fileName.Substring(TEMPORARY_PREFIX.Length));
        }

        private bool CreateTemporaryVideoDir(string fileName)
        {
            var dir = GetTemporaryVideoDir(fileName);

            try
            {
                CreateDir(dir);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot create directory to save the temp video {0}", fileName);

                return false;
            }
            return true;
        }

        private bool CreateCompletedVideoDir(string fileName)
        {
            var dir = GetCompletedVideoDir(fileName);

            try
            {
                CreateDir(dir);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot create directory to save the temp video {0}", fileName);

                return false;
            }

            return true;
        }

        private bool CreateShortVideoDir(string fileName)
        {
            var dir = GetShortVideoDir(fileName);

            try
            {
                CreateDir(dir);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot create directory to save the temp video {0}", fileName);

                return false;
            }

            return true;
        }

        private bool CreateThumbnailDir(string fileName)
        {
            var dir = GetThumbnailDir(fileName);

            try
            {
                CreateDir(dir);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot create directory {0}", fileName);

                return false;
            }

            return true;
        }

        private string GetTemporaryVideoDir(string fileName)
        {
            var videoDir = GetFileNameWithNoExtension(fileName);

            return Path.Combine(env.WebRootPath, VIDEO_TEMPORARY_PATH, videoDir);
        }

        private string GetCompletedVideoDir(string fileName)
        {
            return Path.Combine(env.WebRootPath, VIDEO_COMPLETED_PATH, fileName);
        }

        private string GetShortVideoDir(string fileName)
        {
            var shortVideoDir = GetFileNameWithNoExtension(fileName);

            return Path.Combine(env.WebRootPath, SHORT_VIDEO_PATH, shortVideoDir);
        }

        private string GetThumbnailDir(string fileName)
        {
            return Path.Combine(env.WebRootPath, VIDEO_THUMBNAIL_PATH, fileName);
        }

        private string GetFileNameWithNoExtension(string fileName)
        {
            var mimeLength = GetMime(fileName).Length;

            return fileName.Substring(0, fileName.Length - (mimeLength + 1));
        }

        private string GetPublicPath(string localPath)
        {
            return localPath.Substring(env.WebRootPath.Length);
        }

        private void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Cannot create directory {0}", dir);
                }
            }
        }

        private bool DeleteDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);

                    return true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Directory not deleted");

                    return false;
                }
            }
            return false;
        }

        private string GetMime(string fileName)
        {
            return fileName.Split('.').Last();
        }
    }
}
