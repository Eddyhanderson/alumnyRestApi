using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{    
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService topicService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public TopicController(ITopicService topicService,
            IMapper mapper, IUriService uriService)
        {
            this.topicService = topicService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.TopicRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TopicRequest topicRequest)
        {
            if (topicRequest == null) return BadRequest();

            string route = ApiRoutes.DisciplineTopicRoutes.Get;

            var topic = mapper.Map<Topic>(topicRequest);

            var creationResult = await topicService.CreateAsync(topic);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<TopicResponse>
            {
                Data = mapper.Map<TopicResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.TopicRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] TopicQuery topicQuery)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var topics = await topicService.GetTopicsAsync(filter, topicQuery);

            var topicsResponse = mapper.Map<List<TopicResponse>>(topics);

            var pageResponse = new PageResponse<TopicResponse>(topicsResponse);

            for (int i = 0; i < pageResponse.Data.Count(); i++)
            {
                await SetAnalytics(pageResponse.Data.ElementAt(i));
            }

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: topicsResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.TopicRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.TopicRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var topic = await topicService.GetTopicAsync(Id);

            if (topic == null) return BadRequest();

            var response = new Response<TopicResponse>(mapper.Map<TopicResponse>(topic));

            await SetAnalytics(response.Data);

            return Ok(response);
        }

        private async Task SetAnalytics(TopicResponse topic)
        {
            topic.AnswerCount = await topicService.AnswerCountAsync(topic.Id);
            topic.CommentCount = await topicService.CommentCountAsync(topic.Id);
            topic.LessonCount = await topicService.LessonCountAsync(topic.Id);
            topic.SolvedQuestionCount = await topicService.SolvedQuestionCountAsync(topic.Id);
            topic.TeacherAnswerCount = await topicService.TeacherAnswerCount(topic.Id);
            topic.QuestionCount = await topicService.QuestionCountAsync(topic.Id);
            topic.OpenLessonCount = await topicService.OpenLessonCountAsync(topic.Id);
        }
    }
}
