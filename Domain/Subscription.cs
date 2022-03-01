using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Subscription
    {
        public string Id { get; set; }

        public string StudantId { get; set; }

        public string FormationEventId { get; set; }

        public string Situation { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }

        [ForeignKey(nameof(FormationEventId))]
        public FormationEvent FormationEvent { get; set; }
    }

}