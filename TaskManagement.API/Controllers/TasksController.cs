using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController: ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string category, [FromQuery] string priority, [FromQuery] bool? isCompleted)
        {
            var result = await _service.GetAllAsync(category, priority, isCompleted);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Title }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            await _service.MarkAsCompletedAsync(id);
            return NoContent();
        }
    }
}
