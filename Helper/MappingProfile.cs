using AutoMapper;
using PetShopApi.Models;
using shoppetApi.DTO;

namespace shoppetApi.Helper
{
    public class MappingProfile : Profile
    {
       public MappingProfile() {
            CreateMap<UserRegistrationDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));
        }

    }
}
