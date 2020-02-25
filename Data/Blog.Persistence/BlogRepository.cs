using Blog.Configuration.Core;
using Blog.Db.Models;
using Blog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Threading;

namespace Blog.Persistence
{
    public class BlogRepository : GenericMongoRepository<BlogModel>, IBlogRepository
    {
        private bool? bypassDocumentValidation;
        public BlogRepository(IBlogAppMongoDbSetting appSettings) : base(appSettings)
        {

        }

        public async Task<long> AddComment(string blogId, CommentModel comment)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Push(x => x.Comments, comment);

            var x = (await _collection.UpdateOneAsync(filter, update));

            return x.ModifiedCount;
        }

        public async Task<long> AddRating(string blogId, int rating, UserContextModel user)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Push(x => x.Ratings, rating.ToString());

            var x = (await _collection.UpdateOneAsync(filter, update));

            return x.ModifiedCount;
        }

        public async Task<long> DislikeBlog(string blogId, UserContextModel user)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Push(x => x.DisLike, user);

            var x = (await _collection.UpdateOneAsync(filter, update));

            return x.ModifiedCount;
        }

        public async Task<bool> IsBlogAlreadyLikedByUser(string blogId, UserContextModel user)
        {
            bool isLiked = false;
            var existFilter = Builders<BlogModel>.Filter.And(
                    Builders<BlogModel>.Filter.ElemMatch(x => x.Like, l => l.UserName == user.UserName),
                    Builders<BlogModel>.Filter.Eq(x => x.Id, blogId)
                    );

            var check = (await _collection.FindAsync(existFilter)).ToList();

            if (check != null && check.Count() > 0)
            {
                isLiked = true;
            }
            return isLiked;
        }


        public async Task<long> LikeBlog(string blogId, UserContextModel user)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Push(x => x.Like, user);

            var x = (await _collection.UpdateOneAsync(filter, update));

            return x.ModifiedCount;

        }

        public async Task<long> UnLikeBlog(string blogId, UserContextModel user)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Pull(x => x.Like, user);

            var x = (await _collection.UpdateOneAsync(filter, update));

            return x.ModifiedCount;

        }

        public async Task<long> IncreaseViewCount(string blogId)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = new BsonDocument("$inc", new BsonDocument { { "ViewCount", 1 } });
            var result = await _collection.FindOneAndUpdateAsync(filter, update);
            return result.ViewCount;
        }

        public async Task<long> IncreaseViewCountFromSlug(string slug)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Slug, slug);
            var update = new BsonDocument("$inc", new BsonDocument { { "ViewCount", 1 } });
            var result = await _collection.FindOneAndUpdateAsync(filter, update);
            return result.ViewCount;
        }

        public async Task<BlogModel> GetBlogBySlug(string slug)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Slug, slug);
            var entity = await _collection.FindAsync<BlogModel>(filter);
            return entity.FirstOrDefault();
        }

        public async Task<IList<BlogModel>> GetBlogsWithTags(List<string> tags)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.Tags.Any(b => tags.Contains(b)));
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetBlogswithTag(string tag)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.Tags.Any(b => b.Equals(tag)));
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetBlogsByCategory(string category)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.Category == category);
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetBlogsByCreator(string createdById)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.CreatedBy.UserId, createdById);
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<long> UpdateBlogContent(string blogId, string content, CancellationToken cancellationToken = default)
        {
            var filter = Builders<BlogModel>.Filter.Eq(x => x.Id, blogId);
            var update = Builders<BlogModel>.Update.Set(x => x.Content, content).Set(x => x.Updated, DateTime.Now);
            var result = await _collection.UpdateOneAsync(filter, update, new UpdateOptions { BypassDocumentValidation = bypassDocumentValidation }, cancellationToken);
            return result.ModifiedCount;
        }

        public async Task<IList<BlogModel>> GetMatchingBlog(string searchTerm)
        {
            var filter = Builders<BlogModel>.Filter.Regex(model => model.Title, new BsonRegularExpression(searchTerm, "i"));
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetBlogsLikedByUser(UserContextModel user)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.Like.Any(u => u.UserId == user.UserId));
            return await _collection.Find<BlogModel>(filter).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetPopularBlogs(int count)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.IsDeleted == false);
            return await _collection.Find<BlogModel>(filter).SortByDescending(s => s.ViewCount).Limit(count).ToListAsync();
        }

        public async Task<IList<BlogModel>> GetRecentBlogs(int count)
        {
            var filter = Builders<BlogModel>.Filter.Where(x => x.IsDeleted == false);
            return await _collection.Find<BlogModel>(filter).SortByDescending(s => s.Updated).Limit(count).ToListAsync();
        }

        public async Task<List<AggregateCountEntity>> GetBlogsCountByCategory()
        {
            List<AggregateCountEntity> entities = new List<AggregateCountEntity>();
            var filter = Builders<BlogModel>.Filter.Where(x => x.IsDeleted == false);
            var res = await _collection.Aggregate().SortByCount(b => b.Category).ToListAsync();
            foreach (var index in res)
            {
                entities.Add(new AggregateCountEntity() { Id = index.Id, Count = index.Count });
            }
            return entities;

        }
    }
}