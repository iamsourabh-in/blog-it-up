using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class UserViewModel
    {
        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string PicPath { get; set; }
    }
}
