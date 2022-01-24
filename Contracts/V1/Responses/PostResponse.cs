using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class PostResponse
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserPictureProfilePath { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string PostType { get; set; }

        public string CommentableId { get; set; }

        public DateTime CreateAt { get; set; }

        public string Situation { get; set; }
    }
}
