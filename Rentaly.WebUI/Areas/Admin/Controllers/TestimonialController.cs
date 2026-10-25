using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public async Task<IActionResult> TestimonialList()
        {
            var values = await _testimonialService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(Testimonial testimonial)
        {
            await _testimonialService.TInsertAsync(testimonial);
            return RedirectToAction("TestimonialList");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveTestimonial(int id)
        {
            var testimonial = await _testimonialService.TGetByIdAsync(id);

            if (testimonial == null)
            {
                return NotFound();
            }

            testimonial.IsApproved = true;
            testimonial.Status = true;

            await _testimonialService.TUpdateAsync(testimonial);

            TempData["Success"] = "Yorum onaylandı ve aktif edildi.";

            return RedirectToAction(nameof(TestimonialList));
        }


        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            await _testimonialService.TDeleteAsync(id);
            return RedirectToAction("TestimonialList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTestimonial(Testimonial testimonial)
        {
            await _testimonialService.TUpdateAsync(testimonial);
            return RedirectToAction("TestimonialList");
        }
    }
}