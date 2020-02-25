using Blog.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities
{
    public class DBLogEntity : MongoEntityBase
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }
    }
}
