using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Foundation
{
    /// <summary>  
    /// A non-instantiable base entity which defines   
    /// mongo entity.  
    /// </summary>  
    public abstract class MongoEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? DeletedOn { get; set; }

    }
}
