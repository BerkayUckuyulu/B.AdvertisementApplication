using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Common.Enums;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.UI.Extensions;
using B.AdvertisementApp.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace B.AdvertisementApp.UI.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAdvertisementAppUserService _advertisementAppUserService;

        public AdvertisementController(IAppUserService appUserService, IAdvertisementAppUserService advertisementAppUserService)
        {
            _appUserService = appUserService;
            _advertisementAppUserService = advertisementAppUserService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Send(int advertisementId)
        {
            var userId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);
            var userResponse = await _appUserService.GetByIdAsync<AppUserListDto>(userId);


            ViewBag.GenderId = userResponse.Data.GenderId;

            var items = Enum.GetValues(typeof(MilitaryStatusType));

            var list = new List<MilitaryStatusListDto>();

            foreach (int item in items)
            {
                list.Add(new MilitaryStatusListDto
                {
                    Id = item,
                    Definition = Enum.GetName(typeof(MilitaryStatusType), item)
                });
            }

            ViewBag.MilitaryStatus = new SelectList(list, "Id", "Definition");


            return View(new AdvertisementAppUserCreateModel
            {
                AdvertisementId = advertisementId,
                AppUserId = userId,

            });
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Send(AdvertisementAppUserCreateModel advertisementAppUserCreateModel)
        {
            AdvertisementAppUserCreateDto dto = new();

            if (advertisementAppUserCreateModel.CvFile != null)
            {
                var fileName = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(advertisementAppUserCreateModel.CvFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cvFiles", fileName + extName);
                var stream = new FileStream(path, FileMode.Create);
                await stream.CopyToAsync(stream);
                dto.CvPath = path;
            }

            dto.AdvertisementAppUserStatusId = advertisementAppUserCreateModel.AdvertisementAppUserStatusId;
            dto.AdvertisementId = advertisementAppUserCreateModel.AdvertisementId;
            dto.AppUserId = advertisementAppUserCreateModel.AppUserId;
            dto.EndDate = advertisementAppUserCreateModel.EndDate;
            dto.MilitaryStatusId = advertisementAppUserCreateModel.MilitaryStatusId;
            dto.WorkExperience = advertisementAppUserCreateModel.WorkExperience;


            var response = await _advertisementAppUserService.CreateAsync(dto);
            if (response.ResponseType == Common.ResponseType.ValidationError)
            {
                foreach (var item in response.ValidationErrors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var userId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);
                var userResponse = await _appUserService.GetByIdAsync<AppUserListDto>(userId);


                ViewBag.GenderId = userResponse.Data.GenderId;

                var items = Enum.GetValues(typeof(MilitaryStatusType));

                var list = new List<MilitaryStatusListDto>();

                foreach (int item in items)
                {
                    list.Add(new MilitaryStatusListDto
                    {
                        Id = item,
                        Definition = Enum.GetName(typeof(MilitaryStatusType), item)
                    });
                }

                ViewBag.MilitaryStatus = new SelectList(list, "Id", "Definition");

                return View(advertisementAppUserCreateModel);
            }
            else
            {
                return RedirectToAction("HumanResource", "Home");
            }


        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var list = await _advertisementAppUserService.GetList(AdvertisementAppUserStatusType.Başvuruldu);
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus(int advertisementAppUserId, AdvertisementAppUserStatusType type)
        {

            await _advertisementAppUserService.SetStatusAsync(advertisementAppUserId, type);
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovedList()
        {
            var list = await _advertisementAppUserService.GetList(AdvertisementAppUserStatusType.Mülakat);
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectedList()
        {
            var list = await _advertisementAppUserService.GetList(AdvertisementAppUserStatusType.Olumsuz);
            return View(list);
        }
    }
}
