using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using TodoList.Api.Data;
using TodoList.Api.Data.Models;
using TodoList.Api.Services;

namespace TodoList.Tests.TestFramework {
    public abstract class EntityFrameworkTest : IDisposable {
        private Mock<IUserIdProvider> _userIdProviderMock;
        protected ApplicationUser CurrentUser { get; set; }
        protected ApplicationDbContext DbContext { get; set; }

        public EntityFrameworkTest() {
            this.SetupApplicationDbContext();
            this.SeedCurrentUser();
        }

        public void Dispose() {
            this.DbContext.Dispose();
        }

        private void SetupApplicationDbContext() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoList")
                .Options;
            this._userIdProviderMock = new Mock<IUserIdProvider>();

            this.DbContext = new ApplicationDbContext(options, this._userIdProviderMock.Object);
        }

        private void SeedCurrentUser() {
            this.CurrentUser = new ApplicationUser();
            this.DbContext.ApplicationUsers.Add(CurrentUser);
            this.DbContext.SaveChanges();

            this._userIdProviderMock.Setup(x => x.GetUserId()).Returns(this.CurrentUser.Id);
        }
    }
}
