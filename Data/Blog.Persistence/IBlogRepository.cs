using Blog.Db.Models;
using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Persistence
{
    public interface IBlogRepository : IGenericMongoRepository<BlogModel>
    {
        Task<bool> IsBlogAlreadyLikedByUser(string blogId, UserContextModel user);
        Task<long> LikeBlog(string blogId, UserContextModel user);

        Task<long> UnLikeBlog(string blogId, UserContextModel user);
        Task<long> DislikeBlog(string blogId, UserContextModel user);

        Task<long> AddComment(string blogId, CommentModel comment);

        Task<long> AddRating(string blogId, int rating, UserContextModel user);

        Task<long> IncreaseViewCount(string blogId);

        Task<long> IncreaseViewCountFromSlug(string slug);

        Task<BlogModel> GetBlogBySlug(string slug);

        Task<IList<BlogModel>> GetBlogsWithTags(List<string> tags);

        Task<IList<BlogModel>> GetBlogswithTag(string tag);

        Task<IList<BlogModel>> GetBlogsByCategory(string category);

        Task<IList<BlogModel>> GetBlogsByCreator(string createdById);

        Task<long> UpdateBlogContent(string blogId, string content, CancellationToken cancellationToken = default);

        Task<IList<BlogModel>> GetMatchingBlog(string searchTerm);

        Task<IList<BlogModel>> GetBlogsLikedByUser(UserContextModel user);

        Task<IList<BlogModel>> GetPopularBlogs(int count);

        Task<IList<BlogModel>> GetRecentBlogs(int count);

        Task<List<AggregateCountEntity>> GetBlogsCountByCategory();


    }
}
