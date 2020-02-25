using Blog.Foundation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Db.Models
{
    public class BlogModel : MongoEntityBase
    {
       
        public string BlogImage { get; set; }
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        public List<string> Tags { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Ratings { get; set; }

        public List<UserContextModel> Like { get; set; }

        public List<UserContextModel> DisLike { get; set; }


        public List<CommentModel> Comments { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        public UserContextModel CreatedBy { get; set; }
        public long ViewCount { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Updated { get; set; }
    }
}
