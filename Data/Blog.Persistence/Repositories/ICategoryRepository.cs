using Blog.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Persistence.Repositories
{
    public interface ICategoryRepository : IGenericMongoRepository<CategoryModel>
    {

    }
}
