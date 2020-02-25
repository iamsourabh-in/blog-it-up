using Blog.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Tests
{
    [TestFixture]
    public class TestBlogService
    {
        public Mock<IBlogService> _blogService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IBlogService, BlogService>();
            var serviceProvider = services.BuildServiceProvider();
          //  _blogService = serviceProvider.GetRequiredService<IBlogService>();
        }

        [Test]
        public void DoesUserExist_ForValidUserId_ReturnsTrue()
        {

        }
    }
}
