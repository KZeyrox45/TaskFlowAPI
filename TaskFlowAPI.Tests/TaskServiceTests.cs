using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task GetByIdAsync_ShouldReturnRepositoryResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedTask = new TaskItem { Id = id, Title = "Sample" };
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expectedTask);
            var service = new TaskService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTask.Id, result!.Id);
            mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnUserTasks()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), UserId = userId, Title = "Task 1" },
                new TaskItem { Id = Guid.NewGuid(), UserId = userId, Title = "Task 2" }
            };
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(expectedTasks);
            var service = new TaskService(mockRepo.Object);

            // Act
            var result = (await service.GetByUserIdAsync(userId)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal(userId, t.UserId));
            mockRepo.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "A" },
                new TaskItem { Id = Guid.NewGuid(), Title = "B" },
                new TaskItem { Id = Guid.NewGuid(), Title = "C" }
            };
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedTasks);
            var service = new TaskService(mockRepo.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

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

        [Fact]
        public async Task UpdateAsync_ShouldSetUpdatedAt_AndCallRepository()
        {
            // Arrange
            var mockRepo = new Mock<ITaskRepository>();
            var service = new TaskService(mockRepo.Object);
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Updated" };

            // Act
            await service.UpdateAsync(task);

            // Assert
            Assert.NotNull(task.UpdatedAt);
            mockRepo.Verify(r => r.UpdateAsync(It.Is<TaskItem>(t => t.Id == task.Id)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenTaskExists_ShouldDeleteTask()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskItem { Id = id, Title = "To delete" };
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);
            var service = new TaskService(mockRepo.Object);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenTaskNotFound_ShouldNotCallDelete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);
            var service = new TaskService(mockRepo.Object);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<TaskItem>()), Times.Never);
        }
    }
}
