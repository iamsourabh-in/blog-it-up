using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Web.Models;
using Blog.Services;
using Blog.Web.Filters;
using Blog.Web.Base;
using Blog.Entities;

namespace Blog.Web.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class HomeController : BlogBaseController
    {
        private readonly ILogger<HomeController> _logger;
        ICategoryService _categoryService;
        IBlogService _blogService;
        public HomeController(ILogger<HomeController> logger, IBlogService blogService, ICategoryService categoryService)
        {
            _logger = logger;
            _blogService = blogService;
            _categoryService = categoryService;
        }

        [CacheResource]
        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            UserContextEntity entity = new UserContextEntity();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                entity.UserName = HttpContext.User.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                entity.UserPicPath = HttpContext.User.Claims.Where(c => c.Type == "PicPath").Select(c => c.Value).FirstOrDefault();
                entity.UserId = HttpContext.User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault();
                model.LikedBlogs = await _blogService.GetBlogsLikedByUser(entity);
                model.AllBlogs = await _blogService.GetAll();
                model.Popular = await _blogService.GetPopularBlogs(3);
                model.Recent = await _blogService.GetRecentBlogs(5);
            }
            else
            {
                model.Popular = await _blogService.GetPopularBlogs(3);
                model.Recent = await _blogService.GetRecentBlogs(5);
                model.Blogs = await _blogService.GetAll();
                model.AllBlogs = await _blogService.GetAll();
            }
            var x = await _blogService.GetBlogsCountByCategory();
            model.Categories = await _categoryService.GetAll();
            // throw new NullReferenceException();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    public partial class ViewClassNew {

        public int A { get; set; }
    }
}
