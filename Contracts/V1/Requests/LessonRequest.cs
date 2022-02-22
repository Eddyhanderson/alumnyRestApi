using Alumni.Helpers;
using System.ComponentModel.DataAnnotations;

namespace alumni.Contracts.V1.Requests
{
    public class LessonRequest
    {
        public string Id { get; set; }

        [Required]
        public string ModuleId { get; set; }

        public string VideoId { get; set; }

        public string ArticleId { get; set; }

        public string Picture { get; set; }

        public string ManifestPath { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public int Sequence { get; set; }

        public int Views { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.LessonTypeRegex)]
        public string LessonType { get; set; }
    }
}
