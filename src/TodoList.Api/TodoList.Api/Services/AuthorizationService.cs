using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoList.Api.Data.Dtos.Request;
using TodoList.Api.Data.Dtos.Response;
using TodoList.Api.Data.Models;

namespace TodoList.Api.Services {
    public interface IAuthorizationService {
        Task<IdentityResult> Register(UserRegistrationRequestModel registrationRequest);
        Task<UserLoginResponseModel> Login(UserLoginRequestModel loginRequest);
    }

    public class AuthorizationService : IAuthorizationService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthorizationService(UserManager<ApplicationUser> userManager, IConfiguration configuration) {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<IdentityResult> Register(UserRegistrationRequestModel registrationRequest) {
            ApplicationUser applicationUser = new ApplicationUser() {
                Email = registrationRequest.Email,
                UserName = registrationRequest.Email,
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName
            };

            return await this._userManager.CreateAsync(applicationUser, registrationRequest.Password);
        }

        public async Task<UserLoginResponseModel> Login(UserLoginRequestModel loginRequest) {
            bool hasValidCredentials = await this.AreCredentialsValid(loginRequest);
            if (!hasValidCredentials) {
                return new UserLoginResponseModel() {
                    Succeeded = false
                };
            }

            ApplicationUser user = await this._userManager.FindByNameAsync(loginRequest.Email);
            string token = this.GenerateToken(user);

            return new UserLoginResponseModel() {
                Succeeded = true,
                Token = token,
                Email = loginRequest.Email
            };
        }

        private string GenerateToken(ApplicationUser user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        private async Task<bool> AreCredentialsValid(UserLoginRequestModel loginRequest) {
            ApplicationUser applicationUser = await this._userManager.FindByNameAsync(loginRequest.Email);

            return await this._userManager.CheckPasswordAsync(applicationUser, loginRequest.Password);
        }
    }
}