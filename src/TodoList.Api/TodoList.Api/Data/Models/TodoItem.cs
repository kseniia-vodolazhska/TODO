using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TodoList.Api.Data.Models {
    public class TodoItem {
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SortOrder { get; set; }
    }
}