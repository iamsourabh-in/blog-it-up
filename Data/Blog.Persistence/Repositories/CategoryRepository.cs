using Blog.Configuration.Core;
using Blog.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Persistence.Repositories
{
    public class CategoryRepository : GenericMongoRepository<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(IBlogAppMongoDbSetting appSettings) : base(appSettings)
        {

        }

    }
}
