using Alumni.Helpers;
using System.ComponentModel.DataAnnotations;

namespace alumni.Contracts.V1.Requests
{
    public class LessonRequest
    {
        public string Id { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }

        public string PostId { get; set; }

        public string VideoId { get; set; }

        public string ArticleId { get; set; }        

        [Required]
        public string TopicId { get; set; }

        public string BackgroundPhotoPath { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public int Sequence { get; set; }

        public int Views { get; set; }

        [Required]
        public bool Public { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.LessonTypeRegex)]
        public string LessonType { get; set; }
    }
}
