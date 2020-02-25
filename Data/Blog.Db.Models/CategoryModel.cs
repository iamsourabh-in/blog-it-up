using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Db.Models
{
    public class CategoryModel : MongoEntityBase
    {
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ShortCode { get; set; }
        public bool IsEnabled { get; set; }
        public string Route { get; set; }
    }
}
