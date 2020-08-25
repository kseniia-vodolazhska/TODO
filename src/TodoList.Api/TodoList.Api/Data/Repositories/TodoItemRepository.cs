using TodoList.Api.Data.Models;
using System.Linq;

namespace TodoList.Api.Data.Repositories {
    public interface ITodoItemRepository : IRepository<TodoItem> {
        int GetNextSortOrder();
        TodoItem[] Search(string searchText);

    }

    public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository {
        public TodoItemRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public int GetNextSortOrder() {
            int highestSortOrder = this._dbContext.TodoItems
                .Select(x => x.SortOrder)
                .DefaultIfEmpty(0)
                .Max();

            return highestSortOrder + 1;
        }

        public TodoItem[] Search(string searchText)
        {
            return GetAll()
                .Where(x => x.Title.ToLower().Contains(searchText.ToLower()))
                .ToArray();
        }
    }
}