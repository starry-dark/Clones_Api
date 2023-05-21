using Models.Dtos;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<string>> Register(RegisterDto user);
        Task<BaseResponse<LoginResponse>> Login(LoginRequest request);
        Task<BaseResponse<IEnumerable<UserDto>>> ListUsers();
    }
}
