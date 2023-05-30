using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Dtos;
using Models.Enums;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IConfiguration config, IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
        }

        public async Task<BaseResponse<string>> Register(RegisterDto user)
        {
            var identityUser = _mapper.Map<User>(user);

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, Roles.User.ToString());
                return new BaseResponse<string>().Success("Registration successful!", code:201);
            }
            throw new ArgumentException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return new BaseResponse<LoginResponse>().Error("Invalid credentials!", code:400);

            if(!user.IsActive)
                return new BaseResponse<LoginResponse>().Error("Inactive user. Please contact administrator!", code: 400);

            var result = new LoginResponse()
            {
                Id = user.Id,
                Token = await GenerateToken(user)
            };
            return new BaseResponse<LoginResponse>().Success("Login successful!", data: result);
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> ListUsers()
        {
            var result = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());
            var activeUsers = result.Where(x => x.IsActive);

            if(!activeUsers.Any())
                return new BaseResponse<IEnumerable<UserDto>>().Success("No registered active users!");

            var users = _mapper.Map<IEnumerable<UserDto>>(activeUsers);
            return new BaseResponse<IEnumerable<UserDto>>().Success("Users successfully retrieved!", data: users);
        }

        public async Task<BaseResponse<string>> Deactivate(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseResponse<string>().Error("No user found!", code: 400);
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return new BaseResponse<string>().Success("User deactivated successfully!");
        }

        public async Task<BaseResponse<string>> Activate(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseResponse<string>().Error("No user found!", code: 400);
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return new BaseResponse<string>().Success("User activated successfully!");
        }

        private async Task<string> GenerateToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Specifying JWTSecurityToken Parameters
            var token = new JwtSecurityToken
            (audience: _config["Jwt:Audience"],
             issuer: _config["Jwt:Issuer"],
             claims: authClaims,
             expires: DateTime.Now.AddHours(2),
             signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
