using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Models.Dtos;

namespace Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserDto>();
            CreateMap<AddCredentialRequest, Credential>();
            CreateMap<RegisterDto, User>()
                .ForMember(x => x.IsActive, y => y.MapFrom(src => true))
                .ForMember(x => x.TenantId, y => y.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
