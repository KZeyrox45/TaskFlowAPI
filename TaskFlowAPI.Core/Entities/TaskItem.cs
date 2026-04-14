using System;

namespace TaskFlowAPI.Core.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }

    public enum TaskStatus
    {
        Created,
        InProgress,
        Completed,
        Cancelled
    }
}
