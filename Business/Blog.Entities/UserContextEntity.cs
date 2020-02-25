using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities
{
    public class UserContextEntity
    {
        public string UserName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string UserPicPath { get; set; }
    }
}
