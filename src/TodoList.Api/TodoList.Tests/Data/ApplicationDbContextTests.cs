using Microsoft.EntityFrameworkCore;
using TodoList.Api.Data.Models;
using TodoList.Tests.TestFramework;
using Xunit;

namespace TodoList.Tests.Data {
    public class ApplicationDbContextTests : EntityFrameworkTest {
        [Fact]
        public async void OnModelCreating_ShouldFilterTodoItemsOnApplicationUser_WhenPerformingAnyQuery() {
            // Given: A in memory database context with two users - current user and other user
            ApplicationUser otherUser = new ApplicationUser();
            this.DbContext.ApplicationUsers.Add(otherUser);

            // When: We add another user and give both users a single todo item
            this.DbContext.TodoItems.Add(new TodoItem() { ApplicationUser = this.CurrentUser });
            this.DbContext.TodoItems.Add(new TodoItem() { ApplicationUser = otherUser });
            this.DbContext.SaveChanges();
            
            // Then: The TodoItems DbSet should only return the item that belong to the current user
            Assert.Equal(1, await this.DbContext.TodoItems.CountAsync());
        }
    }
}
