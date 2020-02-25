using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities
{
    public class CommentEntity : MongoEntityBase
    {
        public string CommentText { get; set; }

        public DateTime Created { get; set; }

        public UserContextEntity User { get; set; }
    }
}
