using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.Configuration.Core;
using Blog.Foundation;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Blog.Persistence
{

    /// <summary>  
    /// A MongoDB repository. Maps to a collection with the same name  
    /// as type TEntity.  
    /// </summary>  
    /// <typeparam name=”T”>Entity type for this repository</typeparam>  
    public class GenericMongoRepository<TEntity> : IGenericMongoRepository<TEntity> where TEntity : MongoEntityBase
    {
        protected IBlogAppMongoDbSetting _appSettings;

        protected IMongoDatabase _database;
        protected IMongoCollection<TEntity> _collection;

        private readonly string _epochTime;
        private bool? bypassDocumentValidation;
        public GenericMongoRepository(IBlogAppMongoDbSetting appSettings)
        {
            _appSettings = appSettings;
            GetDatabase();
            GetCollection();
        }

        public async Task<string> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<string> UpdateAsync(string id, TEntity entity)
        {
            if (entity.Id == null)
                return await InsertAsync(entity);

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            var update = Builders<TEntity>.Update.Set(x => x.IsDeleted, true).Set(x => x.DeletedOn, DateTime.Now);

            var x = await _collection.UpdateOneAsync(filter, update);

            return x.ModifiedCount.ToString();
        }

        public async Task<string> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var update = Builders<TEntity>.Update.Set(x => x.IsDeleted, true).Set(x => x.DeletedOn, DateTime.Now);

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { BypassDocumentValidation = bypassDocumentValidation }, cancellationToken);

            return id;
        }

        public IList<TEntity> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _collection
            .AsQueryable<TEntity>()
            .Where(predicate.Compile())
            .ToList();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.IsDeleted, false);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var entity = (await _collection.FindAsync<TEntity>(filter));
            return entity.FirstOrDefault();
        }

        #region Private Helper Methods  
        private void GetDatabase()
        {
            var client = new MongoClient(_appSettings.ConnectionString);
            _database = client.GetDatabase(_appSettings.DatabaseName);
        }

        private void GetCollection()
        {
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        #endregion
    }
}
