using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Data.Dtos;
using TodoList.Api.Data.Models;
using TodoList.Api.Data.Repositories;
using Xunit;

namespace TodoList.Tests.Controllers {
    public class TodoItemControllerTests {
        [Fact]
        public void Get_ShouldReturnNotFound_WhenTodoItemIsNull() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(null);
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = controller.Get(1);

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_ShouldReturnOkResult_WhenTodoItemIsFound() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(new TodoItem());
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = controller.Get(1);

            // Then
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenTodoItemIsNull() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(null);
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = await controller.Update(1, new TodoItemDto());

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnCreated_WhenTodoItemIsFound() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(new TodoItem());
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = await controller.Update(1, new TodoItemDto());

            // Then
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact] 
        public async Task Delete_ShouldReturnNotFound_WhenTodoItemDoesNotExist() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(null);
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = await controller.Delete(1);

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenTodoItemExists() {
            // Given
            var repositoryMock = this.GetTodoItemRepositoryMock(new TodoItem());
            var mapperMock = new Mock<IMapper>();
            TodoItemController controller = new TodoItemController(repositoryMock, mapperMock.Object);

            // When
            IActionResult result = await controller.Delete(1);

            // Then
            Assert.IsType<OkResult>(result);
        }

        private ITodoItemRepository GetTodoItemRepositoryMock(TodoItem getByIdResult) {
            var repositoryMock = new Mock<ITodoItemRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(getByIdResult);
            return repositoryMock.Object;
        }
    }
}