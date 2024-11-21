using AutoMapper;
using PetShopApi.Models;
using shoppetApi.DTO;

namespace shoppetApi.Helper
{
    public class MappingProfile : Profile
    {
       public MappingProfile() {
            CreateMap<UserRegistrationDTO, User>()
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                 .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<SpeciesDTO, Species>();
       }

    }
}
