using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Category;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Application.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color
            });
        }

        public async Task AddAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Color = dto.Color
            };

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
        }
    }
}
