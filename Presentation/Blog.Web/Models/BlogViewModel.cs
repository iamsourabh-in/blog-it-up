using Blog.Entities;
using Blog.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class BlogViewModel
    {


        public BlogViewModel()
        {

        }
        public List<CategoryEntity> Categories { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public List<string> Ratings { get; set; }

        public List<UserContextEntity> Like { get; set; }

        public List<UserContextEntity> DisLike { get; set; }

        [Required]
        public string Category { get; set; }

        public string Content { get; set; }

        [Required]
        public string Tags { get; set; }

        public DateTime CreatedOn { get; set; }

        // User Details
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPicPath { get; set; }


        public async Task Initialize(ICategoryService _categoryCollection, HttpContext context)
        {
            this.CreatedOn = DateTime.Now;
            this.UserId = context.User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault();
            this.UserName = context.User.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
            this.UserPicPath = context.User.Claims.Where(c => c.Type == "PicPath").Select(c => c.Value).FirstOrDefault();
            this.Categories = await _categoryCollection.GetAll();
        }
    }
}
