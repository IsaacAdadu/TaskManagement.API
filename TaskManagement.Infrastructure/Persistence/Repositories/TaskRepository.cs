using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(string category = null, string priority = null, bool? isCompleted = null)
        {
            var query = _context.Tasks.Include(t => t.Category).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(t => t.Category.Name == category);

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse(priority, true, out PriorityLevel parsed))
                query = query.Where(t => t.Priority == parsed);

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            return await query.ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _context.Tasks.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
