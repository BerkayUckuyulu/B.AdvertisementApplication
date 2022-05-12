using AutoMapper;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.UI.Models;

namespace B.AdvertisementApp.UI.Mappings.Automapper
{
    public class UserCreateModelProfile:Profile
    {
        public UserCreateModelProfile()
        {
            CreateMap<UserCreateModel, AppUserCreateDto>().ReverseMap();
        }
    }
}
