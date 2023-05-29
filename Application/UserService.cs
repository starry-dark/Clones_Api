using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration config, IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
        }

        public async Task<BaseResponse<string>> Register(RegisterDto user)
        {
            var identityUser = _mapper.Map<IdentityUser>(user);

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, Roles.User.ToString());
                return new BaseResponse<string>().Success("Registration successful!");
            }
            throw new ArgumentException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return new BaseResponse<LoginResponse>().Error("Invalid credentials!", code:400);

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
            if(!result.Any())
                return new BaseResponse<IEnumerable<UserDto>>().Success("No registered users!");

            var users = _mapper.Map<IList<UserDto>>(result);
            return new BaseResponse<IEnumerable<UserDto>>().Success("Users successfully retrieved!", data: users);
        }

        private async Task<string> GenerateToken(IdentityUser user)
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
