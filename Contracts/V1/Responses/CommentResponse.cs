using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class CommentResponse
    {
        public string Id { get; set; }

        public string ComentableId { get; set; }

        public string Content { get; set; }

        public string PostId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserPhoto { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
