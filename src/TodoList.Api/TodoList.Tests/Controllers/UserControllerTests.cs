using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoList.Api.Controllers;
using TodoList.Api.Data.Dtos.Request;
using TodoList.Api.Data.Dtos.Response;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Tests.Controllers {
    public class UserControllerTests {
        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenIdentityResultSucceededIsFalse() {
            // Given
            var authorizationService = this.GetAuthorizationServiceMock(false, IdentityResult.Failed());
            var userController = new UserController(authorizationService);

            // When
            IActionResult result = await userController.Register(new UserRegistrationRequestModel());

            // Then
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ShouldReturnCreatedStatusCode_WhenRegistrationSuccessfull() {
            // Given
            var authorizationService = this.GetAuthorizationServiceMock(true, IdentityResult.Success);
            var userController = new UserController(authorizationService);

            // When
            IActionResult result = await userController.Register(new UserRegistrationRequestModel());

            // Then
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenLoginFails() {
            // Given
            var authorizationService = this.GetAuthorizationServiceMock(false, IdentityResult.Failed());
            var userController = new UserController(authorizationService);

            // When
            IActionResult result = await userController.Login(new UserLoginRequestModel());

            // Then
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnCreatedStatusCode_WhenLoginSuccessfull() {
            // Given
            var authorizationService = this.GetAuthorizationServiceMock(true, IdentityResult.Success);
            var userController = new UserController(authorizationService);

            // When
            IActionResult result = await userController.Login(new UserLoginRequestModel());

            // Then
            Assert.IsType<OkObjectResult>(result);
        }

        private IAuthorizationService GetAuthorizationServiceMock(bool loginResult, IdentityResult registrationMethodResult) {
            var authorizationService = new Mock<IAuthorizationService>();
            authorizationService.Setup(x => x.Login(It.IsAny<UserLoginRequestModel>()))
                .Returns(Task.FromResult(
                    new UserLoginResponseModel() {
                        Succeeded = loginResult
                    })
                );
            authorizationService.Setup(x => x.Register(It.IsAny<UserRegistrationRequestModel>())).Returns(Task.FromResult(registrationMethodResult));

            return authorizationService.Object;
        }
    }
}