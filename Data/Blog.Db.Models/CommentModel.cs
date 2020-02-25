using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Db.Models
{
    public class CommentModel : MongoEntityBase
    {
        public string CommentText { get; set; }

        public DateTime Created { get; set; }

        public UserContextModel User { get; set; }

    }
}
