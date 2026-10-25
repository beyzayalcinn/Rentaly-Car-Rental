using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;

        public CarController(
            ICarService carService,
            IBrandService brandService,
            ICarModelService carModelService,
            ICategoryService categoryService,
            IBranchService branchService)
        {
            _carService = carService;
            _brandService = brandService;
            _carModelService = carModelService;
            _categoryService = categoryService;
            _branchService = branchService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetListAsync();

            return View(cars);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var car = await _carService.TGetByIdAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(car.BrandId, car.ModelId);
                return View(car);
            }

            await _carService.TInsertAsync(car);

            TempData["Success"] = "Araç başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var car = await _carService.TGetByIdAsync(id);

            if (car == null)
                return NotFound();

            await LoadDropdowns(car.BrandId, car.ModelId);

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Car car)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(car.BrandId, car.ModelId);
                return View(car);
            }

            await _carService.TUpdateAsync(car);

            TempData["Success"] = "Araç başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carService.TGetByIdAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            await _carService.TDeleteAsync(id);

            TempData["Success"] = "Araç başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
        private async Task LoadDropdowns(
     int? selectedBrandId = null,
     int? selectedModelId = null)
        {
            var brands = await _brandService.TGetListAsync();
            var models = await _carModelService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();

            ViewBag.Brands = new SelectList(
                brands,
                "BrandId",
                "BrandName",
                selectedBrandId);

            ViewBag.Models = new SelectList(
                models,
                "CarModelId",
                "ModelName",
                selectedModelId);

            ViewBag.AllModels = models;

            ViewBag.Categories = new SelectList(
                categories,
                "CategoryId",
                "CategoryName");

            ViewBag.Branches = new SelectList(
                branches,
                "BranchId",
                "BranchName");
        }
    }
}