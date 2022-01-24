using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TopicResponse
    {
        public string Id { get; set; }

        public string CommentableId { get; set; }

        public string PhotoProfile { get; set; }

        public string Description { get; set; }

        public string DisciplineTopicId { get; set; }

        public string TopicId { get; set; }

        public string TeacherPlaceId { get; set; }

        public string TeacherPlaceName { get; set; }

        public string TeacherPlaceProfilePhoto { get; set; }

        public string TeacherPlaceProfileDisciplineName { get; set; }

        public string DisciplineName { get; set; }

        public string PostId { get; set; }

        public string DisciplineTopicName { get; set; }

        public DateTime CreationAt { get; set; }

        public int OpenLessonCount { get; set; }

        public int LessonCount { get; set; }

        public int AnswerCount { get; set; }

        public int QuestionCount { get; set; }

        public int SolvedQuestionCount { get; set; }

        public int TeacherAnswerCount { get; set; }

        public int CommentCount { get; set; }
    }
}
