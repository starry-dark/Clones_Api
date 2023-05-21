using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Models.Dtos;

namespace Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<AddCredentialRequest, Credential>();
            CreateMap<RegisterDto, IdentityUser>();
            CreateMap<IdentityUser, UserDto>();
        }
    }
}
