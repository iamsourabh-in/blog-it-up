using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Persistence
{
    public interface IGenericMongoRepository<TRequestEntity> where TRequestEntity : MongoEntityBase
    {
        Task<string> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<IList<TRequestEntity>> GetAllAsync();
        Task<TRequestEntity> GetByIdAsync(string id);
        Task<string> InsertAsync(TRequestEntity entity);
        IList<TRequestEntity> SearchForAsync(Expression<Func<TRequestEntity, bool>> predicate);
        Task<string> UpdateAsync(string id, TRequestEntity entity);
    }
}