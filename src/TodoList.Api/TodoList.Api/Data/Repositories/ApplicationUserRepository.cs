using System.Linq;
using TodoList.Api.Data.Models;

namespace TodoList.Api.Data.Repositories {
    public interface IApplicationUserRepository : IRepository<ApplicationUser> {
        bool Exists(string email);
    }

    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository {
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public bool Exists(string email) {
            return this._dbContext.ApplicationUsers.Any(x => x.Email == email);
        }
    }
}