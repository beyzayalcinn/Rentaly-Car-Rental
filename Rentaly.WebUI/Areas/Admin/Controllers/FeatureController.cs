using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public async Task<IActionResult> FeatureList()
        {
            var values = await _featureService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateFeature()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature(Feature feature)
        {
            await _featureService.TInsertAsync(feature);
            return RedirectToAction("FeatureList");
        }

        public async Task<IActionResult> DeleteFeature(int id)
        {
            await _featureService.TDeleteAsync(id);
            return RedirectToAction("FeatureList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFeature(int id)
        {
            var value = await _featureService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeature(Feature feature)
        {
            await _featureService.TUpdateAsync(feature);
            return RedirectToAction("FeatureList");
        }
    }
}