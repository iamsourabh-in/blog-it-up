using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities;
using Blog.Services;
using Blog.Web.Filters;
using Blog.Web.Models.ApiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ActionController : ControllerBase
    {
        private readonly IBlogService _blogService;

        private readonly IHostEnvironment _hostingEnvironment;

        public ActionController(IBlogService blogService, IHostEnvironment hostingEnvironment)
        {
            _blogService = blogService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        [Route("LikeBlog")]
        public async Task<ActionResult> PostLikeBlog(ActionApiModel model)
        {
            var count = await _blogService.LikeBlog(model.BlogId, model.User);
            return Ok(count);
        }
        [HttpPost]
        [Route("UnLikeBlog")]
        public async Task<ActionResult> UnLikeBlog(ActionApiModel model)
        {
            var count = await _blogService.UnLikeBlog(model.BlogId, model.User);
            return Ok(count);
        }

        [HttpPost]
        [Route("DisLikeBlog")]
        public async Task<IActionResult> DisLikeBlog(ActionApiModel model)
        {
            var count = await _blogService.DislikeBlog(model.BlogId, model.User);
            return Ok(count);
        }
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment(ActionApiModel model)
        {
            model.Comment.Created = DateTime.Now;
            var count = await _blogService.AddComment(model.BlogId, model.Comment);
            return Ok(count);
        }

        [HttpPost]
        [Route("AddRating")]
        public async Task<IActionResult> AddRating(ActionApiModel model)
        {
            var count = await _blogService.AddRating(model.BlogId, model.Rating, model.User);
            return Ok(count);
        }

        [HttpPost]
        [Route("UpdateContent")]
        public async Task<IActionResult> UpdateContent(ActionApiModel model)
        {
            var count = await _blogService.UpdateBlogContent(model.BlogId, model.Content);
            return Ok(count);
        }

        [HttpGet]
        [Route("SearchBlog")]
        public async Task<IActionResult> SearchBlog(string id)
        {
            var blogs = await _blogService.GetMatchingBlog(id);
            return Ok(blogs);
        }
    }
}