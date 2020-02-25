using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Configuration.Core
{
    public class BlogAppMongoDbSetting : IBlogAppMongoDbSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBlogAppMongoDbSetting
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
