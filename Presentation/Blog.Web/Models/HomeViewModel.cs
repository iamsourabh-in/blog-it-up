using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class HomeViewModel
    {
        public List<CategoryEntity> Categories;
        public List<BlogEntity> Blogs;
        public List<BlogEntity> AllBlogs;
        public List<BlogEntity> LikedBlogs;
        public List<BlogEntity> Popular;
        public List<BlogEntity> Recent;
        public HomeViewModel()
        {

        }

    }
}
