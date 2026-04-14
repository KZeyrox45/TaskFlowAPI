using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskFlowAPI.Application.Services;
using TaskFlowAPI.Core.Entities;

namespace TaskFlowAPI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = Guid.Parse(userIdStr);
            var tasks = await _taskService.GetByUserIdAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            
            var userIdStr = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || task.UserId != Guid.Parse(userIdStr)) return Forbid();
            
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                UserId = Guid.Parse(userIdStr)
            };
            
            await _taskService.AddAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            
            var userIdStr = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || task.UserId != Guid.Parse(userIdStr)) return Forbid();
            
            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            
            await _taskService.UpdateAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            
            var userIdStr = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || task.UserId != Guid.Parse(userIdStr)) return Forbid();
            
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }

    public record CreateTaskRequest(string Title, string? Description);
    public record UpdateTaskRequest(string Title, string? Description, TaskFlowAPI.Core.Entities.TaskStatus Status);
}
