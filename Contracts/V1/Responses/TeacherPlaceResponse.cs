using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TeacherPlaceResponse
    {
        public string Id { get; set; }

        public string TeacherId { get; set; }

        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        public string Price { get; set; }

        public DateTime CreateAt { get; set; }

        public string Description { get; set; }

        public string TeacherPictureProfilePath { get; set; }

        public string DisciplineId { get; set; }

        public string DisciplineName { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public string TeacherPlaceCode { get; set; }

        public string SchoolId { get; set; }

        public string SchoolName { get; set; }

        public string SchoolShortName { get; set; }

        public string SchoolPictureProfilePath { get; set; }

        public string Name { get; set; }

        public string ProfilePhotoPath { get; set; }

        public bool Opened { get; set; }

        public int AnswerCount { get; set; }

        public int StudantsCount { get; set; }

        public int LessonsCount { get; set; }

        public int TopicCount { get; set; }

        public int QuestionsCount { get; set; }

        public int StudantAnswerCount { get; set; }

        public int TeacherAnswerCount { get; set; }

        public int SolvedQuestionCount { get; set; }

    }
}
