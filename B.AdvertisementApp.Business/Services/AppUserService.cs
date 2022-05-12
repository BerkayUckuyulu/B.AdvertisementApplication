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
using B.AdvertisementApp.Business.Extensions;

namespace B.AdvertisementApp.Business.Services
{
    public class AppUserService:Service<AppUserCreateDto,AppUserUpdateDto,AppUserListDto,AppUser>,IAppUserService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUserCreateDto> _createDtoValidator;
        public AppUserService(IMapper mapper,IValidator<AppUserCreateDto> createDtoValidator,IValidator<AppUserUpdateDto> updateDtoValidator,IUow uow):base(mapper,createDtoValidator,uow,updateDtoValidator)
        {
            _uow = uow;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
        }
        public async Task<IResponse<AppUserCreateDto>> CreateWithRoleAsync(AppUserCreateDto appUserCreateDto,int roleId)
        {
            var validationResult = _createDtoValidator.Validate(appUserCreateDto);
            if (validationResult.IsValid)
            {
                var appUser = _mapper.Map<AppUser>(appUserCreateDto);
                //appUser.AppUserRoles.Add(new AppUserRole
                //{
                //    AppUser = appUser,
                //    AppRoleId=roleId
                //});

                await _uow.GetRepository<AppUser>().CreateAsync(appUser);
                await _uow.GetRepository<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppRoleId = roleId,
                    AppUser = appUser
                });
                await _uow.SaveChangesAsync();

                return new Response<AppUserCreateDto>(ResponseType.Success, appUserCreateDto);
            }
            return new Response<AppUserCreateDto>(appUserCreateDto, validationResult.ConvertToCustomValidationError());

        }
    }
}
