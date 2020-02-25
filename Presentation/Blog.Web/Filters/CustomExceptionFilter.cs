using Blog.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<CustomAuthFilterAttribute> _logger;

        public CustomExceptionFilter(
       IHostEnvironment hostingEnvironment,
       IModelMetadataProvider modelMetadataProvider, ILogger<CustomAuthFilterAttribute> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {

            // Get All Properties.
            //_logger.LogError(Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.Exception));

            string token = filterContext.HttpContext.Request.Cookies["Token"];
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            var errorModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? filterContext.HttpContext.TraceIdentifier, StatusCode = "500" };


        

            IActionResult result = new RedirectResult("/anonymus/Error?statusCode=500");

            //var viewResult = new ViewResult
            //{
            //    ViewName = "~/Views/Shared/Error.cshtml",
            //    ViewData = new ViewDataDictionary(_modelMetadataProvider, filterContext.ModelState),
            //};

            //filterContext.Result = new ViewResult();

            //filterContext.Result.ViewData.Add("Exception", filterContext.Exception.Message);
            //filterContext.Result.ViewData.Add("Controller", controllerName);
            //filterContext.Result.ViewData.Add("ActivityID", Activity.Current?.Id);

            // TODO: Pass additional detailed data via ViewData
            filterContext.Result = result;
            filterContext.ExceptionHandled = true;


        }
    }
}
