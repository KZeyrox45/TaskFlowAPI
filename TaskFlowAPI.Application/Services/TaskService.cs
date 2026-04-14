using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskFlowAPI.Core.Entities;
using TaskFlowAPI.Core.Interfaces;

namespace TaskFlowAPI.Application.Services
{
    public interface ITaskService
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(Guid id);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId)
        {
            return await _taskRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            task.CreatedAt = DateTime.UtcNow;
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                await _taskRepository.DeleteAsync(task);
            }
        }
    }
}
