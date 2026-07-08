using AutoMapper;
using UserService.Application.Features.Users.Commands.Login;
using UserService.Domain.Entities;

namespace UserService.Application.Mappings
{
    public class LoginMapping : Profile
    {
        public LoginMapping()
        {
            CreateMap<User, UserInfoResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role)); 
        }
    }
}