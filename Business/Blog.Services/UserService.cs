using AutoMapper;
using Blog.Db.Models;
using Blog.Entities;
using Blog.Foundation.Helper;
using Blog.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class UserService : IUserService
    {

        IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<string> Create(UserEntity entity)
        {
            entity.Password = Crypt.GetMD5Hash(entity.Password);
            entity.CreatedBy = "Admin";
            entity.CreatedOn = DateTime.Now;
            entity.Roles = "User,Blogger";
            var model = _mapper.Map<UserModel>(entity);
            return await _userRepository.InsertAsync(model);
        }

        public async Task<string> Delete(string id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<List<UserEntity>> GetAll()
        {
            List<UserEntity> entities = new List<UserEntity>();
            var model = (await _userRepository.GetAllAsync());
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<UserEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<UserEntity> GetById(string id)
        {
            UserEntity entities = new UserEntity();
            var model = (await _userRepository.GetByIdAsync(id));
            if (model != null)
            {
                entities = _mapper.Map<UserEntity>(model);
            }
            return entities;
        }

        public async Task<string> Update(string id, UserEntity entity)
        {
            var model = _mapper.Map<UserModel>(entity);
            return await _userRepository.UpdateAsync(id, model);
        }

        public async Task<UserEntity> Authenticate(string Email, string Password)
        {
            if (String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException();

            UserEntity entities = null;
            var model = await _userRepository.Authenticate(Email, Password);

            if (!WOW.IsNull(model))
            {
                entities = _mapper.Map<UserEntity>(model);
            }
            return entities;

        }

        public async Task<bool> IsEmailAlreadyRegistered(string email)
        {
            return await _userRepository.IsEmailAlreadyRegistered(email);
        }
    }
}
