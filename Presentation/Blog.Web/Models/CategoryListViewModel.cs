using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Models
{
    public class CategoryListViewModel
    {
        public IEnumerable<AggregateCountEntity> CategoryBlogsCount { get; set; }

        public IEnumerable<Blog.Entities.CategoryEntity> Categories { get; set; }



        public void MapAll()
        {
            foreach (var cat in Categories)
            {
                foreach (var CatCount in CategoryBlogsCount)
                {
                    if (cat.Name.Equals(CatCount.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        cat.Count = CatCount.Count;
                    }
                }
            }
        }
    }
}
