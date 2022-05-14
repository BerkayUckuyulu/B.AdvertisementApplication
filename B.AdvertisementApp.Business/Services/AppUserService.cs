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
using B.AdvertisementApp.Dto;

namespace B.AdvertisementApp.Business.Services
{
    public class AppUserService : Service<AppUserCreateDto, AppUserUpdateDto, AppUserListDto, AppUser>, IAppUserService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUserCreateDto> _createDtoValidator;
        private readonly IValidator<AppUserLogInDto> _logInDtoValidator;
        public AppUserService(IMapper mapper, IValidator<AppUserCreateDto> createDtoValidator, IValidator<AppUserUpdateDto> updateDtoValidator, IUow uow, IValidator<AppUserLogInDto> logInDtoValidator) : base(mapper, createDtoValidator, uow, updateDtoValidator)
        {
            _uow = uow;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _logInDtoValidator = logInDtoValidator;
        }
        public async Task<IResponse<AppUserCreateDto>> CreateWithRoleAsync(AppUserCreateDto appUserCreateDto, int roleId)
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
        public async Task<IResponse<AppUserListDto>> CheckUserAsync(AppUserLogInDto appUserLogInDto)
        {
            var validationResult = _logInDtoValidator.Validate(appUserLogInDto);

            if (validationResult.IsValid)
            {
                var appUser = await _uow.GetRepository<AppUser>().GetByFilterAsync(x => x.UserName == appUserLogInDto.UserName && x.Password == appUserLogInDto.Password);
                if (appUser != null)
                {
                    var appUserListDto = _mapper.Map<AppUserListDto>(appUser);
                    return new Response<AppUserListDto>(ResponseType.Success, appUserListDto);
                }
                else
                {
                    return new Response<AppUserListDto>(ResponseType.NotFound, "Kullanıcı adı veya şifre hatalı.");
                }

            }
            return new Response<AppUserListDto>(ResponseType.ValidationError, "Kullanıcı adı veya şifre boş olamaz");
        }
        public async Task<IResponse<List<AppRoleListDto>>> GetRolesByUserIdAsync(int userId)
        {
            var roles = await _uow.GetRepository<AppRole>().GetAllAsync(x => x.AppUserRoles.Any(x => x.AppUserId == userId));
            if (roles == null)
            {
                return new Response<List<AppRoleListDto>>(ResponseType.NotFound, "İlgili Rol Bulunamadı.");
            }
            var dto= _mapper.Map<List<AppRoleListDto>>(roles);

            return new Response<List<AppRoleListDto>>(ResponseType.Success, dto);
        }
    }
}
