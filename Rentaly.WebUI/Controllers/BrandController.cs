using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            if (!ModelState.IsValid)
                return View(brand);

            await _brandService.TInsertAsync(brand);

            return RedirectToAction("BrandList");
        }
        public async Task<IActionResult> BrandList()
        {
            var values = await _brandService.TGetListAsync();
            return View(values);
        }
    }
} 
