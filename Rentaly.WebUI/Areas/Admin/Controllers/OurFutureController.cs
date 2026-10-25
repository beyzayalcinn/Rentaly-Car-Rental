using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OurFutureController : Controller
    {
        private readonly IOurFutureService _ourFutureService;

        public OurFutureController(IOurFutureService ourFutureService)
        {
            _ourFutureService = ourFutureService;
        }

        public async Task<IActionResult> OurFutureList()
        {
            var values = await _ourFutureService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateOurFuture()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOurFuture(OurFuture ourFuture)
        {
            await _ourFutureService.TInsertAsync(ourFuture);
            return RedirectToAction("OurFutureList");
        }

        public async Task<IActionResult> DeleteOurFuture(int id)
        {
            await _ourFutureService.TDeleteAsync(id);
            return RedirectToAction("OurFutureList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOurFuture(int id)
        {
            var value = await _ourFutureService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOurFuture(OurFuture ourFuture)
        {
            await _ourFutureService.TUpdateAsync(ourFuture);
            return RedirectToAction("OurFutureList");
        }
    }
}