using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models.ApiModel
{
    public class ActionApiModel
    {
        public string BlogId { get; set; }
        public int Rating { get; set; }

        public String Content { get; set; }
        public UserContextEntity User { get; set; }

        public CommentEntity Comment { get; set; }
    }
}
