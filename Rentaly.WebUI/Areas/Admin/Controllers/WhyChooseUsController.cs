using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WhyChooseUsController : Controller
    {
        private readonly IWhyChooseUsService _service;

        public WhyChooseUsController(IWhyChooseUsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> WhyChooseUsList()
        {
            var values = await _service.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateWhyChooseUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWhyChooseUs(WhyChooseUs entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _service.TInsertAsync(entity);

            return RedirectToAction(nameof(WhyChooseUsList));
        }

        public async Task<IActionResult> DeleteWhyChooseUs(int id)
        {
            await _service.TDeleteAsync(id);

            return RedirectToAction(nameof(WhyChooseUsList));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWhyChooseUs(int id)
        {
            var value = await _service.TGetByIdAsync(id);

            if (value == null)
                return NotFound();

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWhyChooseUs(WhyChooseUs entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _service.TUpdateAsync(entity);

            return RedirectToAction(nameof(WhyChooseUsList));
        }
    }
}