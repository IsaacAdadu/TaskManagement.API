using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllAsync(string category = null, string priority = null, bool? isCompleted = null);
        Task<TaskDto> GetByIdAsync(int id);
        Task CreateAsync(CreateTaskDto dto);
        Task UpdateAsync(int id, UpdateTaskDto dto);
        Task DeleteAsync(int id);
        Task MarkAsCompletedAsync(int id);
    }
}
