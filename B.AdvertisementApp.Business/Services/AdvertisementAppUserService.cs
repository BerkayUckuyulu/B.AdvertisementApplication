using AutoMapper;
using B.AdvertisementApp.Business.Extensions;
using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Common;
using B.AdvertisementApp.Common.Enums;
using B.AdvertisementApp.DataAccess.UnitOfWork;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<AdvertisementAppUserListDto>> GetList(AdvertisementAppUserStatusType statusType)
        {
            var query = _uow.GetRepository<AdvertisementAppUser>().GetQuery();
           var list= await query.Include(x => x.Advertisement).Include(x => x.AdvertisementAppUserStatus).Include(x => x.MilitaryStatus).Include(x => x.AppUser).ThenInclude(x=>x.Gender).Where(x => x.AdvertisementAppUserStatusId == (int)statusType).ToListAsync();
          return  _mapper.Map<List<AdvertisementAppUserListDto>>(list);
        }

        public async Task SetStatusAsync(int advertisementAppUserId,AdvertisementAppUserStatusType type)
        {
            //var unchanged = await _uow.GetRepository<AdvertisementAppUser>().FindAsync(advertisementAppUserId);
            //var changed= await _uow.GetRepository<AdvertisementAppUser>().GetByFilterAsync(x=>x.Id==advertisementAppUserId);
            //changed.Id = advertisementAppUserId;
            //changed.AdvertisementAppUserStatusId = (int)type;
            //_uow.GetRepository<AdvertisementAppUser>().Update(changed, unchanged);

             var query=_uow.GetRepository<AdvertisementAppUser>().GetQuery();
            var entity =await query.SingleOrDefaultAsync(x => x.Id == advertisementAppUserId);
            entity.AdvertisementAppUserStatusId=((int)type);
            await _uow.SaveChangesAsync();
        }
    }
}
