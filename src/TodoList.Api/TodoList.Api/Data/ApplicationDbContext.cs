using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.Data.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        private readonly IUserIdProvider _userIdProvider;

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserIdProvider userIdProvider) :
            base(options) {
            this._userIdProvider = userIdProvider;
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<TodoItem>().HasQueryFilter(x => x.ApplicationUserId == this._userIdProvider.GetUserId());
        }
    }
}