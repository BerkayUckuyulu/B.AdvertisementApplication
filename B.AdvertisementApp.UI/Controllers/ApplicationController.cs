using B.AdvertisementApp.Business.Interfaces;
using B.AdvertisementApp.Dtos;
using B.AdvertisementApp.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B.AdvertisementApp.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationController : Controller
    {
        private readonly IAdvertisementService _advertisementService;

        public ApplicationController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        public async Task<IActionResult> List()
        {
            var response = await _advertisementService.GetAllAsync();

            return this.ResponseView(response);
        }
        public IActionResult Create()
        {
            return View(new AdvertisementCreateDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementCreateDto advertisementCreateDto)
        {
           var response=await _advertisementService.CreateAsync(advertisementCreateDto);
            return this.ResponseRedirectAction(response,"List");
        }
        public async Task<IActionResult> Update(int id)
        {
            var response=await _advertisementService.GetByIdAsync<AdvertisementUpdateDto>(id);
            return this.ResponseView(response);

        }
        [HttpPost]
        public async Task<IActionResult> Update(AdvertisementUpdateDto advertisementUpdateDto)
        {
            var response = await _advertisementService.UpdateAsync(advertisementUpdateDto);
            return this.ResponseRedirectAction(response, "List");

        }

        public async Task<IActionResult> Remove(int id)
        {
            var response = await _advertisementService.RemoveAsync(id);
            return this.ResponseRedirectAction(response, "List");

        }
    }
}
