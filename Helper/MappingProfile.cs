using AutoMapper;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Enums;

namespace shoppetApi.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDTO, User>()
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                 .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<SpeciesDTO, Species>()
                .ForMember(dest => dest.SpeciesName, opt => opt.MapFrom(src => HelperMethods.ApplyTitleCase(src.SpeciesName)));

            CreateMap<BreedDTO, Breed>()
                .ForMember(dest => dest.BreedName, opt => opt.MapFrom(src => HelperMethods.ApplyTitleCase(src.BreedName)));

            CreateMap<PetDTO, Pet>()
                .ForMember(dest => dest.PetGender, opt => opt.MapFrom(src => HelperMethods.ApplyTitleCase(src.PetGender)));

            CreateMap<AddOrderDTO, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => HelperMethods.ApplyTitleCase(OrderStatusEnum.pending.ToString())));
        }

    }
}
