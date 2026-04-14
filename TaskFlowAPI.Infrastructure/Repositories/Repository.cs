using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Core.Entities;
using TaskFlowAPI.Core.Interfaces;
using TaskFlowAPI.Infrastructure.Persistence;

namespace TaskFlowAPI.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.Include(t => t.User).ToListAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public class UserRepository : IUserRepository
    {
        private readonly TaskFlowDbContext _context;

        public UserRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
