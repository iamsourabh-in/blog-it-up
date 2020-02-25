using Blog.Foundation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Db.Models
{
    public class UserContextModel : MongoEntityBase
    {
        public string UserName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string UserPicPath { get; set; }
    }
}
