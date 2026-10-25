using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly ICategoryService _categoryService;
        private readonly IRentalService _rentalService;
        private readonly ICustomerService _customerService;

        public HomeController(
            ICarService carService,
            IBrandService brandService,
            ICarModelService carModelService,
            ICategoryService categoryService,
            IRentalService rentalService,
            ICustomerService customerService)
        {
            _carService = carService;
            _brandService = brandService;
            _carModelService = carModelService;
            _categoryService = categoryService;
            _rentalService = rentalService;
            _customerService = customerService;
        }


        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetListAsync();
            var brands = await _brandService.TGetListAsync();
            var models = await _carModelService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();
            var rentals = await _rentalService.TGetListAsync();
            var customers = await _customerService.TGetListAsync();

            ViewBag.TotalCars = cars?.Count ?? 0;
            ViewBag.TotalBrands = brands?.Count ?? 0;
            ViewBag.TotalModels = models?.Count ?? 0;
            ViewBag.TotalCategories = categories?.Count ?? 0;
            ViewBag.TotalCustomers = customers?.Count ?? 0;
            ViewBag.TotalRentals = rentals?.Count ?? 0;

            ViewBag.PendingRentals = rentals?.Count(x => x.Status == "Pending") ?? 0;
            ViewBag.ApprovedRentals = rentals?.Count(x => x.Status == "Approved") ?? 0;
            ViewBag.RejectedRentals = rentals?.Count(x => x.Status == "Rejected") ?? 0;
            ViewBag.CancelledRentals = rentals?.Count(x => x.Status == "Cancelled") ?? 0;

            ViewBag.TotalRevenue = rentals?
                .Where(x => x.Status == "Approved")
                .Sum(x => x.TotalPrice) ?? 0;

            ViewBag.LatestCars = cars?
                .OrderByDescending(x => x.CarId)
                .Take(5)
                .ToList();

            ViewBag.LatestRentals = rentals?
                .OrderByDescending(x => x.RentalId)
                .Take(5)
                .ToList();

            return View();
        }

    }
}