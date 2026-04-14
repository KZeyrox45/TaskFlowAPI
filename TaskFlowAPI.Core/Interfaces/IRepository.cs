using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskFlowAPI.Core.Entities;

namespace TaskFlowAPI.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
    }

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
