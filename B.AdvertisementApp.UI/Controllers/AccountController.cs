using AutoMapper;
using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.UI.Extensions;
using B.AdvertisementApp.UI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace B.AdvertisementApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenderService _genderService;
        private readonly IValidator<UserCreateModel> _userCreateValidator;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        public AccountController(IGenderService genderService, IValidator<UserCreateModel> userCreateValidator, IAppUserService appUserService, IMapper mapper)
        {
            _genderService = genderService;
            _userCreateValidator = userCreateValidator;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        public async Task<IActionResult> SignUp()
        {
          var response=  await _genderService.GetAllAsync();
            var model = new UserCreateModel
            {
                Genders = new SelectList(response.Data,"Id","Definition")
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateModel model)
        {
            var result=_userCreateValidator.Validate(model);
            if (result.IsValid)
            {
                var appUserCreateDto= _mapper.Map<AppUserCreateDto>(model);
               var createResponse= await _appUserService.CreateWithRoleAsync(appUserCreateDto,2);
                return this.ResponseRedirectAction(createResponse, "SignIn");
                
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var response = await _genderService.GetAllAsync();
            model.Genders = new SelectList(response.Data, "Id", "Definition",model.GenderId);
            return View(model);
        }
    }
}
