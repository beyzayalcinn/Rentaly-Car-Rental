using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IBranchService _branchService;
        private readonly ICarModelService _carModelService;

        public CarController(
            ICarService carService,
            ICategoryService categoryService,
            IBrandService brandService,
            IBranchService branchService,
            ICarModelService carModelService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _brandService = brandService;
            _branchService = branchService;
            _carModelService = carModelService;
        }

        // ARAÇ LİSTESİ + FİLTRELEME
        public async Task<IActionResult> CarList(
            int? brandId,
            int? modelId,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int? branchId)
        {
            // Tüm araçları getir
            var cars = await _carService.TGetListAsync();

            // Filtre seçeneklerini getir
            var brands = await _brandService.TGetListAsync();
            var models = await _carModelService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();

            // MARKA FİLTRESİ
            if (brandId.HasValue)
            {
                cars = cars
                    .Where(x => x.BrandId == brandId.Value)
                    .ToList();
            }

            // MODEL FİLTRESİ
            if (modelId.HasValue)
            {
                cars = cars
                    .Where(x => x.ModelId == modelId.Value)
                    .ToList();
            }

            // KATEGORİ FİLTRESİ
            if (categoryId.HasValue)
            {
                cars = cars
                    .Where(x => x.CategoryId == categoryId.Value)
                    .ToList();
            }

            // MİNİMUM FİYAT
            if (minPrice.HasValue)
            {
                cars = cars
                    .Where(x => x.DailyPrice >= minPrice.Value)
                    .ToList();
            }

            // MAKSİMUM FİYAT
            if (maxPrice.HasValue)
            {
                cars = cars
                    .Where(x => x.DailyPrice <= maxPrice.Value)
                    .ToList();
            }

            // LOKASYON
            if (branchId.HasValue)
            {
                cars = cars
                    .Where(x => x.BranchId == branchId.Value)
                    .ToList();
            }

            // MARKALAR
            ViewBag.Brands = new SelectList(
                brands,
                "BrandId",
                "BrandName",
                brandId);

            // MODELLER
            ViewBag.Models = models;

            // SEÇİLİ MODEL
            ViewBag.SelectedModelId = modelId;

            // KATEGORİLER
            ViewBag.Categories = new SelectList(
                categories,
                "CategoryId",
                "CategoryName",
                categoryId);

            // ŞUBELER
            ViewBag.Branches = new SelectList(
                branches,
                "BranchId",
                "BranchName",
                branchId);

            // FİYATLAR
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(cars);
        }

        // CREATE
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(
                await _categoryService.TGetListAsync(),
                "CategoryId",
                "CategoryName");

            ViewBag.Branches = new SelectList(
                await _branchService.TGetListAsync(),
                "BranchId",
                "BranchName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _categoryService.TGetListAsync(),
                    "CategoryId",
                    "CategoryName");

                ViewBag.Branches = new SelectList(
                    await _branchService.TGetListAsync(),
                    "BranchId",
                    "BranchName");

                return View(car);
            }

            await _carService.TInsertAsync(car);

            TempData["Success"] = "Araç başarıyla eklendi.";

            return RedirectToAction("CarList");
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _carService.TDeleteAsync(id);

            TempData["Success"] = "Araç silindi.";

            return RedirectToAction("CarList");
        }

        // EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var value = await _carService.TGetByIdAsync(id);

            if (value == null)
                return NotFound();

            ViewBag.Categories = new SelectList(
                await _categoryService.TGetListAsync(),
                "CategoryId",
                "CategoryName");

            ViewBag.Branches = new SelectList(
                await _branchService.TGetListAsync(),
                "BranchId",
                "BranchName");

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car car)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _categoryService.TGetListAsync(),
                    "CategoryId",
                    "CategoryName");

                ViewBag.Branches = new SelectList(
                    await _branchService.TGetListAsync(),
                    "BranchId",
                    "BranchName");

                return View(car);
            }

            await _carService.TUpdateAsync(car);

            TempData["Success"] = "Araç güncellendi.";

            return RedirectToAction("CarList");
        }

        // DETAIL

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var car = await _carService.TGetByIdAsync(id);

            if (car == null)
                return NotFound();

            var brands = await _brandService.TGetListAsync();
            var models = await _carModelService.TGetListAsync();

            ViewBag.BrandName = brands
                .FirstOrDefault(x => x.BrandId == car.BrandId)?.BrandName
                ?? "Marka belirtilmemiş";

            ViewBag.ModelName = models
                .FirstOrDefault(x => x.CarModelId == car.ModelId)?.ModelName
                ?? "Model belirtilmemiş";

            return View(car);
        }

        // ESKİ URL İÇİN UYUMLULUK
        [HttpGet]
        public IActionResult CarDetail(int id)
        {
            return RedirectToAction("Detail", new { id = id });
        }
    }

}