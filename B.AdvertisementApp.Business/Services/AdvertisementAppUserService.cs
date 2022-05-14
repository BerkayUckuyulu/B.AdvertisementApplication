using AutoMapper;
using B.AdvertisementApp.Business.Extensions;
using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Common;
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
    public class AdvertisementAppUserService:IAdvertisementAppUserService
    {
        private readonly IUow _uow;
        private readonly IValidator<AdvertisementAppUserCreateDto> _createDtoValidator;
        private readonly IMapper _mapper;
        public AdvertisementAppUserService(IUow uow, IValidator<AdvertisementAppUserCreateDto> createDtoValidator, IMapper mapper)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _mapper = mapper;
        }

        public async Task<IResponse<AdvertisementAppUserCreateDto>> CreateAsync(AdvertisementAppUserCreateDto advertisementAppUserCreateDto)
        {
            var result=_createDtoValidator.Validate(advertisementAppUserCreateDto);
            if (result.IsValid)
            {
                var control=await _uow.GetRepository<AdvertisementAppUser>().GetByFilterAsync(x=>x.AppUserId==advertisementAppUserCreateDto.AppUserId && x.AdvertisementId==advertisementAppUserCreateDto.AdvertisementId);

                if (control == null)
                {
                    var createdAdvertisementAppUser = _mapper.Map<AdvertisementAppUser>(advertisementAppUserCreateDto);
                    await _uow.GetRepository<AdvertisementAppUser>().CreateAsync(createdAdvertisementAppUser);
                    await _uow.SaveChangesAsync();

                    return new Response<AdvertisementAppUserCreateDto>(ResponseType.Success, advertisementAppUserCreateDto);
                }

                

                return new Response<AdvertisementAppUserCreateDto>(ResponseType.ValidationError,"Daha önce bu ilana kayıt yaptınız.");

                
            }
            else
            {
                return new Response<AdvertisementAppUserCreateDto>(advertisementAppUserCreateDto, result.ConvertToCustomValidationError());
            }
        }
    }
}
