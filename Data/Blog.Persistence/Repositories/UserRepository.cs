using Blog.Configuration.Core;
using Blog.Db.Models;
using Blog.Foundation.Helper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Persistence.Repositories
{
    public class UserRepository : GenericMongoRepository<UserModel>, IUserRepository
    {
        public UserRepository(IBlogAppMongoDbSetting appSettings) : base(appSettings)
        {

        }

        public async Task<UserModel> Authenticate(string email, string password)
        {
            UserModel model = null;
            var filter = Builders<UserModel>.Filter.Eq(x => x.Email, email);
            var user = (await _collection.FindAsync<UserModel>(filter)).FirstOrDefault(); ;
            if (!WOW.IsNull(user))
            {

                if (user.Password == password)
                {
                    model = user;
                }
            }
            return model;
        }

        public async Task<bool> IsEmailAlreadyRegistered(string email)
        {
            bool isRegistered = true;
            var filter = Builders<UserModel>.Filter.Eq(x => x.Email, email);
            var user = (await _collection.FindAsync<UserModel>(filter)).FirstOrDefault(); ;
            if (WOW.IsNull(user))
            {
                isRegistered = false;
            }
            return isRegistered;
        }
    }
}
