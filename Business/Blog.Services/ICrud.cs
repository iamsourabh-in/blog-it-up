using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    public interface ICrud<TEntity>
    {
        Task<string> Create(TEntity entity);

        Task<string> Delete(string id);

        Task<string> Update(string id, TEntity entity);

        Task<List<TEntity>> GetAll();

        Task<TEntity> GetById(string id);
    }
}
