using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Blog.Entities;
using Blog.Persistence;
using Blog.Services;
using Blog.Web.Base;
using Blog.Web.Filters;
using Blog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Blog.Web.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class BlogController : BlogBaseController
    {
        IBlogService _blogService;

        private readonly IHostEnvironment _hostingEnvironment;
        ICategoryService _categoryCollection;
        private readonly ILogger<BlogController> _blogError;

        public BlogController(IBlogService blogService, IHostEnvironment hostingEnvironment, ICategoryService categoryCollection, ILogger<BlogController> blogError)
        {
            _blogService = blogService;
            _hostingEnvironment = hostingEnvironment;
            _categoryCollection = categoryCollection;
            _blogError = blogError;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var tenant = RouteData.Values["id"];
            //tenant = tenant == null ? "" : tenant.ToString();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> One(string id)
        {
            await _blogService.IncreaseViewCount(id);
            BlogEntity entity = await _blogService.GetById(id);
            return View("Index", entity);
        }

        [HttpGet]
        public async Task<IActionResult> Slug(string id)
        {
            await _blogService.IncreaseViewCountFromSlug(id);
            BlogEntity entity = await _blogService.GetBlogBySlug(id);
            return View("Index", entity);
        }

        [HttpGet]
        public async Task<IActionResult> Tag(string id)
        {
            List<BlogEntity> entities = await _blogService.GetBlogswithTag(id);
            return View("Tags", entities);
        }

        [CustomAuthFilter]
        [CacheResource]
        [HttpGet]
        public async Task<IActionResult> Category(string id)
        {
            List<BlogEntity> entities = await _blogService.GetBlogsByCategory(id);
            return View("Tags", entities);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BlogViewModel model = new BlogViewModel();
            await model.Initialize(_categoryCollection, HttpContext);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogViewModel model)
        {
            string BUCKET_NAME = "srx-blog-images";
            if (ModelState.IsValid)
            {
                UserContextEntity user = new UserContextEntity() { UserId = model.UserId, UserName = model.UserName, UserPicPath = model.UserPicPath };
                var blog = new Entities.BlogEntity();
                string uniqueFileName = null;

                if (model.ImageFile != null)
                {
                    var response = new PutObjectResponse();
                    var client = new AmazonS3Client(RegionEndpoint.EUWest1);
                    uniqueFileName = Guid.NewGuid().ToString() + '_' + model.ImageFile.FileName;
                    using (var stream = new MemoryStream())
                    {
                        model.ImageFile.CopyTo(stream);

                        var request = new PutObjectRequest
                        {
                            BucketName = BUCKET_NAME,
                            Key = uniqueFileName,
                            InputStream = stream,
                            ContentType = model.ImageFile.ContentType,
                        };

                        response = await client.PutObjectAsync(request);
                    };
                }

                blog.BlogImage = uniqueFileName;
                blog.IsDeleted = false;
                blog.Like = new List<UserContextEntity>();
                blog.DisLike = new List<UserContextEntity>();
                blog.Ratings = new List<string>();
                blog.SubTitle = model.SubTitle;
                blog.Title = model.Title;
                List<string> list = model.Tags.Split(",").ToList();
                blog.Tags = new List<string>();
                foreach (string str in list)
                {
                    blog.Tags.Add(str.Trim());
                }
                blog.Category = model.Category;
                blog.CreatedBy = user;
                blog.CreatedOn = DateTime.Now;
                blog.Content = model.Content;
                await _blogService.Create(blog);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                await model.Initialize(_categoryCollection, HttpContext);
                return View(model);
            }


        }
        [CacheResource]
        public async Task<IActionResult> PopularBlogs(int id = 5)
        {
            int count = id;
            var blog = await _blogService.GetPopularBlogs(count);
            return View(blog);

        }
        [CacheResource]
        public async Task<IActionResult> RecentBlogs(int id = 5)
        {
            int count = id;
            var blog = await _blogService.GetRecentBlogs(count);
            return View(blog);

        }


        public async Task<IActionResult> ViewCommentPartial(string id)
        {
            var Blog = await _blogService.GetById(id);
            return PartialView("_ViewCommentPartial", Blog.Comments);
        }
        public async Task<IActionResult> PostCommentPartial(string id)
        {
            return PartialView("_PostCommentPartial", new CommentViewModel(id));
        }

        public async Task<IActionResult> GetPopularBlogs(string id)
        {
            int onum = 9;
            bool num = int.TryParse(id, out onum);
            var Blog = await _blogService.GetPopularBlogs(num ? onum : 3);
            return PartialView("_PopularBlogPartial", Blog);
        }

        public async Task<IActionResult> GetBlogsCountByCategory()
        {
            var Blog = await _blogService.GetBlogsCountByCategory();
            return PartialView("_CategoryCountPartial", Blog);
        }

    }
}