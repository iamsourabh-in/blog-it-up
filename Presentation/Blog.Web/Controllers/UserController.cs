using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities;
using Blog.Services;
using Blog.Web.Base;
using Blog.Web.Filters;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [CustomAuthFilterAttribute]
    public class UserController : BlogBaseController
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        IUserService _userService { get; }

        public UserController(IUserService userService, IMapper mapper, IBlogService blogService)
        {
            _mapper = mapper;
            _userService = userService;
            _blogService = blogService;
        }


        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userService.GetById(id);
            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        public async Task<IActionResult> MyBlogs(string id)
        {
            List<BlogEntity> entities = await _blogService.GetBlogsByCreator(id);
            return View(entities);
        }

        public async Task<IActionResult> EditBlog(string id)
        {
            BlogEntity entities = await _blogService.GetById(id);
            return View(entities);
        }
    }
}