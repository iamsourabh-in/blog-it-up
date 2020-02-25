using Blog.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Persistence.Repositories
{
    public interface IUserRepository : IGenericMongoRepository<UserModel>
    {
        Task<UserModel> Authenticate(string email, string password);

        Task<bool> IsEmailAlreadyRegistered(string email);
    }
}
