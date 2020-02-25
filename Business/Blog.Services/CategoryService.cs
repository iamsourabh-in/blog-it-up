using AutoMapper;
using Blog.Db.Models;
using Blog.Entities;
using Blog.Persistence;
using Blog.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<string> Create(CategoryEntity entity)
        {
            var model = _mapper.Map<CategoryModel>(entity);
            return await _categoryRepository.InsertAsync(model);
        }

        public async Task<string> Delete(string id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryEntity>> GetAll()
        {
            List<CategoryEntity> entities = new List<CategoryEntity>();
            var model = (await _categoryRepository.GetAllAsync());
            if (model != null && model.Count > 0)
            {
                entities = _mapper.Map<List<CategoryEntity>>(model.ToList());
            }
            return entities;
        }

        public async Task<CategoryEntity> GetById(string id)
        {
            return _mapper.Map<CategoryEntity>(await _categoryRepository.GetByIdAsync(id));
        }

        public async Task<string> Update(string id, CategoryEntity entity)
        {
            var model = _mapper.Map<CategoryModel>(entity);
            return await _categoryRepository.UpdateAsync(id, model);
        }
    }
}
