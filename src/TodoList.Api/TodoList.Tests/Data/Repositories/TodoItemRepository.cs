using System.Threading.Tasks;
using TodoList.Api.Data.Models;
using TodoList.Api.Data.Repositories;
using TodoList.Tests.TestFramework;
using Xunit;

namespace TodoList.Tests.Data.Repositories {
    public class TodoItemRepositoryTests : EntityFrameworkTest {
        [Fact]
        public void GetNextSortOrder_ShouldReturnOne_WhenThereAreNoTodoItems() {
            // Given: A database without any todo items
            TodoItemRepository todoItemRepository = new TodoItemRepository(this.DbContext);

            // When: We call GetNextSortOrder
            int nextSortOrder = todoItemRepository.GetNextSortOrder();

            // Then: The sortorder should be one 
            Assert.Equal(1, nextSortOrder);
        }

        [Fact]
        public async Task GetNextSortOrder_ShouldReturnMaximumSortOrderPlusOne_WhenThereAreExistingTodoItems() {
            // Given: A database with multiple todo items
            TodoItemRepository todoItemRepository = new TodoItemRepository(this.DbContext);
            todoItemRepository.Add(new TodoItem() { ApplicationUser = this.CurrentUser, SortOrder = 1 });
            todoItemRepository.Add(new TodoItem() { ApplicationUser = this.CurrentUser, SortOrder = 2 });
            await todoItemRepository.SaveChangesAsync();

            // When: We call GetNextSortOrder
            int nextSortOrder = todoItemRepository.GetNextSortOrder();

            // Then: The sortorder should be three 
            Assert.Equal(3, nextSortOrder);
        }
    }
}
