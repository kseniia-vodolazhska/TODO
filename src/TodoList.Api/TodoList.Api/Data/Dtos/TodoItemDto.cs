using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Data.Dtos {
    public class TodoItemDto {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int SortOrder { get; set; }
    }
}