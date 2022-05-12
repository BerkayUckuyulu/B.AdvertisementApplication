using AutoMapper;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.Mappings.AutoMapper
{
    public class ProvidedServiceProfile:Profile
    {
        public ProvidedServiceProfile()
        {
            CreateMap<ProvidedService, ProvidedServiceCreateDto>().ReverseMap();    
            CreateMap<ProvidedService, ProvidedServiceUpdateDto>().ReverseMap();    
            CreateMap<ProvidedService, ProvidedServiceListDto>().ReverseMap();    
        }
    }
}
