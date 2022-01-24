using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class LessonResponse
    {
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string TeacherPlaceId { get; set; }

        public string TeacherPlaceName { get; set; }

        public string TeacherPlacePhotoPath { get; set; }

        public string TeacherId { get; set; }

        public string SchoolId { get; set; }
        
        public string SchoolName { get; set; }

        public string PostId { get; set; }

        public string TopicId { get; set; }        
        
        public string DisciplineTopicName { get; set; }

        public string DiscpilineId { get; set; }

        public string DisciplineName { get; set; }

        public string BackgroundPhotoPath { get; set; }

        public string ManifestPath { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
        
        public int Sequence { get; set; }

        public int Views { get; set; }

        public string LessonType { get; set; }

        public string Duration { get; set; }

        public DateTime Date { get; set; }

        public bool Public { get; set; }

        public int QuestionCount { get; set; }

        public int SolvedQuestionCount { get; set; }

        public int TeacherAnswerCount { get; set; }

        public int AnswerCount { get; set; }
    }
}
