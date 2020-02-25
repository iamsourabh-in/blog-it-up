using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.Db.Models;
using Blog.Entities;

namespace Blog.Services
{
    public interface IBlogService : ICrud<BlogEntity>
    {
        Task<long> LikeBlog(string blogId, UserContextEntity user);

        Task<long> UnLikeBlog(string blogId, UserContextEntity user);

        Task<long> DislikeBlog(string blogId, UserContextEntity user);

        Task<long> AddComment(string blogId, CommentEntity comment);

        Task<long> AddRating(string blogId, int rating, UserContextEntity user);

        Task<long> IncreaseViewCount(string blogId);

        Task<BlogEntity> GetBlogBySlug(string slug);

        Task<long> IncreaseViewCountFromSlug(string slug);

        Task<List<BlogEntity>> GetBlogsWithTags(List<string> tags);

        Task<List<BlogEntity>> GetBlogswithTag(string tags);

        Task<List<BlogEntity>> GetBlogsByCategory(string category);

        Task<List<BlogEntity>> GetBlogsByCreator(string createdById);

        Task<long> UpdateBlogContent(string blogId, string content, CancellationToken cancellationToken = default);

        Task<List<BlogEntity>> GetMatchingBlog(string searchTerm);

        Task<List<BlogEntity>> GetBlogsLikedByUser(UserContextEntity user);

        Task<List<BlogEntity>> GetPopularBlogs(int count);

        Task<List<BlogEntity>> GetRecentBlogs(int count);

        Task<List<AggregateCountEntity>> GetBlogsCountByCategory();
    }
}
