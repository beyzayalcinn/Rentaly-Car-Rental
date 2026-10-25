using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IActionResult> AboutList()
        {
            var values = await _aboutService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbout(About about)
        {
            await _aboutService.TInsertAsync(about);
            return RedirectToAction("AboutList");
        }

        public async Task<IActionResult> DeleteAbout(int id)
        {
            await _aboutService.TDeleteAsync(id);
            return RedirectToAction("AboutList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAbout(int id)
        {
            var value = await _aboutService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(About about)
        {
            await _aboutService.TUpdateAsync(about);
            return RedirectToAction("AboutList");
        }
    }
}