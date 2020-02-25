using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class CommentViewModel
    {
        public string BlogId { get; set; }
        public string CommentText { get; set; }

        // User Details
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPicPath { get; set; }

        public CommentViewModel(string blogId)
        {
            this.BlogId = blogId;
        }
    }
}
