using Blog.Db.Models;
using Blog.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    public interface IUserService : ICrud<UserEntity>
    {
        Task<UserEntity> Authenticate(string Email, string Password);
        Task<bool> IsEmailAlreadyRegistered(string email);
    }
}
