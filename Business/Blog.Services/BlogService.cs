using AutoMapper;
using Blog.Db.Models;
using Blog.Entities;
using Blog.Foundation.Helper;
using Blog.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class BlogService : IBlogService
    {
        IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;

        }

        #region CRUD
        public async Task<string> Create(BlogEntity blog)
        {
            //Genrate SLug
            blog.Slug = SlugHelper.GenerateSlug(blog.Title);

            // blog.Content = add for alll image-> style="height:400px;width:auto;";
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BlogModel, BlogEntity>().ReverseMap());
            var model = _mapper.Map<BlogModel>(blog);
            model.Updated = DateTime.Now;
            return await _blogRepository.InsertAsync(model);
        }
        public async Task<List<BlogEntity>> GetAll()
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetAllAsync());
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<BlogEntity> GetById(string id)
        {
            return _mapper.Map<BlogEntity>(await _blogRepository.GetByIdAsync(id));
        }

        public async Task<BlogEntity> GetBlogBySlug(string slug)
        {
            return _mapper.Map<BlogEntity>(await _blogRepository.GetBlogBySlug(slug));
        }

        public async Task<string> Delete(string id)
        {
            return await _blogRepository.DeleteAsync(id);
        }

        public async Task<string> Update(string id, BlogEntity blog)
        {
            var model = _mapper.Map<BlogModel>(blog);
            return (await _blogRepository.UpdateAsync(id, model));
        }
        #endregion

        #region Actions
        public async Task<long> AddComment(string blogId, CommentEntity comment)
        {
            var model = _mapper.Map<CommentModel>(comment);
            return (await _blogRepository.AddComment(blogId, model));
        }

        public async Task<long> AddRating(string blogId, int rating, UserContextEntity user)
        {
            var model = _mapper.Map<UserContextModel>(user);
            return (await _blogRepository.AddRating(blogId, rating, model));
        }


        public async Task<long> DislikeBlog(string blogId, UserContextEntity user)
        {
            var model = _mapper.Map<UserContextModel>(user);
            return (await _blogRepository.DislikeBlog(blogId, model));
        }


        public async Task<long> LikeBlog(string blogId, UserContextEntity user)
        {
            long count = 0;
            var model = _mapper.Map<UserContextModel>(user);
            if (!await _blogRepository.IsBlogAlreadyLikedByUser(blogId, model))
                count = (await _blogRepository.LikeBlog(blogId, model));
            return count;
        }

        public async Task<long> UnLikeBlog(string blogId, UserContextEntity user)
        {
            long count = 0;
            var model = _mapper.Map<UserContextModel>(user);
            if (await _blogRepository.IsBlogAlreadyLikedByUser(blogId, model))
                count = (await _blogRepository.UnLikeBlog(blogId, model));
            return count;
        }

        public async Task<long> IncreaseViewCount(string blogId)
        {
            return (await _blogRepository.IncreaseViewCount(blogId));
        }

        public async Task<long> IncreaseViewCountFromSlug(string slug)
        {
            return (await _blogRepository.IncreaseViewCountFromSlug(slug));
        }
        public async Task<List<BlogEntity>> GetBlogswithTag(string tag)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetBlogswithTag(tag));
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<List<BlogEntity>> GetBlogsWithTags(List<string> tags)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetBlogsWithTags(tags));
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }
        public async Task<List<BlogEntity>> GetBlogsByCategory(string category)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetBlogsByCategory(category));
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<List<BlogEntity>> GetBlogsByCreator(string createdById)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetBlogsByCreator(createdById));
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<long> UpdateBlogContent(string blogId, string content, CancellationToken cancellationToken = default)
        {
            return (await _blogRepository.UpdateBlogContent(blogId, content));
        }

        public async Task<List<BlogEntity>> GetMatchingBlog(string searchTerm)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = (await _blogRepository.GetMatchingBlog(searchTerm));
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<List<BlogEntity>> GetBlogsLikedByUser(UserContextEntity user)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var model = _mapper.Map<UserContextModel>(user);
            var blogs = (await _blogRepository.GetBlogsLikedByUser(model));
            if (blogs != null && blogs.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(blogs.ToList());
            }
            return entities;
        }
        public async Task<List<BlogEntity>> GetPopularBlogs(int count)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var blogs = (await _blogRepository.GetPopularBlogs(count));
            if (blogs != null && blogs.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(blogs.ToList());
            }
            return entities;
        }
        public async Task<List<BlogEntity>> GetRecentBlogs(int count)
        {
            List<BlogEntity> entities = new List<BlogEntity>();
            var blogs = (await _blogRepository.GetRecentBlogs(count));
            if (blogs != null && blogs.Count > 0)
            {
                entities = _mapper.Map<List<BlogEntity>>(blogs.ToList());
            }
            return entities;
        }

        public async Task<List<AggregateCountEntity>> GetBlogsCountByCategory()
        {
            var result = await _blogRepository.GetBlogsCountByCategory();
            return result;
        }
        #endregion


    }
}
