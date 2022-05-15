using AutoMapper;
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
    public class AdvertisementService : Service<AdvertisementCreateDto, AdvertisementUpdateDto, AdvertisementListDto, Advertisement>, IAdvertisementService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;

        public AdvertisementService(IMapper mapper,IValidator<AdvertisementCreateDto> createDtoValidator,IUow uow,IValidator<AdvertisementUpdateDto> updateDtoValidator):base(mapper, createDtoValidator,uow,updateDtoValidator)
        {
            _uow= uow;
            _mapper= mapper;
        }
        public async Task<IResponse<List<AdvertisementListDto>>> GetActiveAsync()
        {
            var data= await _uow.GetRepository<Advertisement>().GetAllAsync(x => x.Status == true, x => x.CreatedDate, Common.Enums.OrderByType.DESC);
            var dtoList = _mapper.Map<List<AdvertisementListDto>>(data);
            
            return new Response<List<AdvertisementListDto>>(ResponseType.Success, dtoList);
        }
    }
}
