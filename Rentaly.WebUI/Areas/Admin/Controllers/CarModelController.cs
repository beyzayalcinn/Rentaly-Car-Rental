using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        public async Task<IActionResult> CarModelList()
        {
            var values = await _carModelService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCarModel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarModel(CarModel carModel)
        {
            await _carModelService.TInsertAsync(carModel);
            return RedirectToAction("CarModelList");
        }

        public async Task<IActionResult> DeleteCarModel(int id)
        {
            await _carModelService.TDeleteAsync(id);
            return RedirectToAction("CarModelList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCarModel(int id)
        {
            var value = await _carModelService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCarModel(CarModel carModel)
        {
            await _carModelService.TUpdateAsync(carModel);
            return RedirectToAction("CarModelList");
        }
    }
}