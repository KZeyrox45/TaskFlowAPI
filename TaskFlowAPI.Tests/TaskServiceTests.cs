using System;
using System.Threading.Tasks;
using Moq;
using TaskFlowAPI.Application.Services;
using TaskFlowAPI.Core.Entities;
using TaskFlowAPI.Core.Interfaces;
using Xunit;

namespace TaskFlowAPI.Tests
{
    public class TaskServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldSetCreatedAt_AndCallRepository()
        {
            // Arrange
            var mockRepo = new Mock<ITaskRepository>();
            var service = new TaskService(mockRepo.Object);
            var task = new TaskItem { Title = "Test Task" };

            // Act
            await service.AddAsync(task);

            // Assert
            Assert.NotEqual(default, task.CreatedAt);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }
    }
}
