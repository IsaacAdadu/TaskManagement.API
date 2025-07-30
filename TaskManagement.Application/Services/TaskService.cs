using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Application.Services
{
    public class TaskService: ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync(string category = null, string priority = null, bool? isCompleted = null)
        {
            var tasks = await _taskRepository.GetAllAsync(category, priority, isCompleted);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority.ToString(),
                Category = t.Category?.Name,
                IsCompleted = t.IsCompleted,
                CreatedDate = t.CreatedDate,
                DueDate = t.DueDate
            });
        }

        public async Task<TaskDto> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority.ToString(),
                Category = task.Category?.Name,
                IsCompleted = task.IsCompleted,
                CreatedDate = task.CreatedDate,
                DueDate = task.DueDate
            };
        }

        public async Task CreateAsync(CreateTaskDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new ArgumentException("Category not found.");

            if (!Enum.TryParse(dto.Priority, true, out PriorityLevel priority))
                throw new ArgumentException("Invalid priority level.");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = priority,
                CategoryId = dto.CategoryId,
                DueDate = dto.DueDate
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new ArgumentException("Category not found.");

            if (!Enum.TryParse(dto.Priority, true, out PriorityLevel priority))
                throw new ArgumentException("Invalid priority level.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = priority;
            task.CategoryId = dto.CategoryId;
            task.IsCompleted = dto.IsCompleted;
            task.DueDate = dto.DueDate;

            await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            await _taskRepository.DeleteAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

        public async Task MarkAsCompletedAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            task.IsCompleted = true;
            await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
        }

    }
}
