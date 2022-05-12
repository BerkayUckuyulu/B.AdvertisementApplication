using AutoMapper;
using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.DataAccess.UnitOfWork;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Business.Services
{
    public class GenderService:Service<GenderCreateDto,GenderUpdateDto,GenderListDto,Gender>,IGenderService
    {
        public GenderService(IMapper mapper,IValidator<GenderCreateDto> createDtoValidator,IUow uow,IValidator<GenderUpdateDto> updateDtoValidator):base(mapper,createDtoValidator,uow,updateDtoValidator)
        {

        }
    }
}
