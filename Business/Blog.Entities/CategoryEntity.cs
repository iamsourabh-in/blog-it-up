using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities
{
    public class CategoryEntity : MongoEntityBase
    {
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ShortCode { get; set; }
        public bool IsEnabled { get; set; }
        public string Route { get; set; }
        public long Count { get; set; }
        public string Desc { get; set; }
    }
}
