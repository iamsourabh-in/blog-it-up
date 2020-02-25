using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities;
using Blog.Persistence;
using Blog.Services;
using Blog.Web.Base;
using Blog.Web.Filters;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CategoryController : BlogBaseController
    {
        private readonly IBlogService _blogService;
        ICategoryService _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryController(IBlogService blogService, ICategoryService categoryCollection, IMapper mapper)
        {
            _blogService = blogService;
            _categoryCollection = categoryCollection;
            _mapper = mapper;
        }
        public async Task<IActionResult> All()
        {
            CategoryListViewModel model = new CategoryListViewModel();
            model.CategoryBlogsCount = await _blogService.GetBlogsCountByCategory();
            model.Categories = await _categoryCollection.GetAll();
            model.MapAll();
            ViewBag.AllCategories = model;
            return View(model);
        }

        // need to move to Blog/Category
        public async Task<IActionResult> One(string id)
        {
            var all = await _categoryCollection.GetAll();
            string tenant = RouteData.Values["id"].ToString();
            ViewData["SelectedCategory"] = tenant;
            ViewBag.AllCategories = all;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryEntity category)
        {
            var all = await _categoryCollection.GetAll();
            string tenant = RouteData.Values["id"].ToString();
            ViewData["SelectedCategory"] = tenant;
            return View();
        }
    }
}