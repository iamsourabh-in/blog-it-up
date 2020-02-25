using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Db.Models
{
    public class DBLogModel : MongoEntityBase
    {
        public string Route { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }
        public string userId { get; set; }
    }
}
