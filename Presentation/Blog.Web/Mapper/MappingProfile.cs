using AutoMapper;
using Blog.Db.Models;
using Blog.Entities;
using Blog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<BlogModel, BlogEntity>().ReverseMap();
            CreateMap<CategoryModel, CategoryEntity>().ReverseMap();
            CreateMap<UserContextModel, UserContextEntity>().ReverseMap();
            CreateMap<CommentModel, CommentEntity>().ReverseMap();
            CreateMap<UserModel, UserEntity>().ReverseMap();

            // Locals
            CreateMap<RegisterViewModel, UserEntity>().ReverseMap();
            CreateMap<UserViewModel, UserEntity>().ReverseMap();
            CreateMap<CategoryBlogCountViewModel, AggregateCountEntity>().ReverseMap();

        }
    }
}
