using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class CommentListModel
    {
        public string CommentText { get; set; }

        public DateTime Created { get; set; }

        public UserContextEntity User { get; set; }
    }
}
