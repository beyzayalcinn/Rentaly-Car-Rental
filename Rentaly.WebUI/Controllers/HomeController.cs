using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAboutService _about;
        private readonly IFeatureService _feature;
        private readonly IWhyChooseUsService _whyChooseUs;
        private readonly IProcessService _process;
        private readonly ITestimonialService _testimonial;
        private readonly IFaqService _faq;
        private readonly IOurFutureService _ourFuture;
        private readonly IAwardService _award;
        private readonly ICarService _car;
        private readonly IBrandService _brand;
        private readonly ICarModelService _carModel;
        private readonly IRentalService _rental;
        private readonly IBranchService _branch;
        private readonly ICategoryService _category;

        public HomeController(
            IAboutService about,
            IFeatureService feature,
            IWhyChooseUsService whyChooseUs,
            IProcessService process,
            ITestimonialService testimonial,
            IFaqService faq,
            IOurFutureService ourFuture,
            IAwardService award,
            ICarService car,
            IBrandService brand,
            ICarModelService carModel,
            IRentalService rental,
            IBranchService branch,
            ICategoryService category)
        {
            _about = about;
            _feature = feature;
            _whyChooseUs = whyChooseUs;
            _process = process;
            _testimonial = testimonial;
            _faq = faq;
            _ourFuture = ourFuture;
            _award = award;
            _car = car;
            _brand = brand;
            _carModel = carModel;
            _rental = rental;
            _branch = branch;
            _category = category;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                About = (await _about.TGetListAsync())
                    .FirstOrDefault(x => x.Status),

                Features = (await _feature.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                WhyChooseUs = (await _whyChooseUs.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Processes = (await _process.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Testimonials = (await _testimonial.TGetListAsync())
    .Where(x => x.Status && x.IsApproved)
    .ToList(),

                Faqs = (await _faq.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                OurFutures = (await _ourFuture.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Awards = (await _award.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Cars = (await _car.TGetListAsync())
                    .Where(x => x.IsActive && x.IsAvailable)
                    .Take(10)
                    .ToList(),

                Brands = await _brand.TGetListAsync(),

                Models = await _carModel.TGetListAsync(),

                Branches = (await _branch.TGetListAsync())
    .Where(b => b.BranchId == 3 || b.BranchId == 4)
    .ToList(),

                Categories = await _category.TGetListAsync()
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Search(
            int branchId,
            int returnBranchId,
            DateTime pickupDate,
            string pickupTime,
            DateTime returnDate,
            string returnTime,
            int? categoryId)
        {
            if (!TimeSpan.TryParse(pickupTime, out var pickupTimeValue))
            {
                pickupTimeValue = new TimeSpan(9, 0, 0);
            }

            if (!TimeSpan.TryParse(returnTime, out var returnTimeValue))
            {
                returnTimeValue = new TimeSpan(9, 0, 0);
            }

            var pickupDateTime = pickupDate.Date.Add(pickupTimeValue);
            var returnDateTime = returnDate.Date.Add(returnTimeValue);

            if (returnDateTime <= pickupDateTime)
            {
                TempData["SearchError"] =
                    "Teslim tarihi ve saati, alýþ tarihi ve saatinden sonra olmalýdýr.";

                return RedirectToAction(nameof(Index));
            }

            var cars = await _car.TGetAllCarsWithCategoryAsync();

            // Yalnýzca aktif ve kiralamaya uygun araçlarý getir.
            var candidateCars = cars
                .Where(c => c.IsActive && c.IsAvailable)
                .Where(c => c.BranchId == branchId)
                .ToList();

            // Þimdilik Ýstanbul Havalimaný þubesi kullanýlacak.
            if (branchId != 3 || returnBranchId != 3)
            {
                TempData["SearchError"] =
                    "Þimdilik yalnýzca Ýstanbul Havalimaný þubesinden araç kiralanabilir.";

                return RedirectToAction(nameof(Index));
            }

            // Kategori seçilmiþse filtrele.
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                candidateCars = candidateCars
                    .Where(c => c.CategoryId == categoryId.Value)
                    .ToList();
            }

            // Kategori seçilmiþse filtrele
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                candidateCars = candidateCars
                    .Where(c => c.CategoryId == categoryId.Value)
                    .ToList();
            }

            var availableCars = new List<Rentaly.EntityLayer.Entities.Car>();

            foreach (var car in candidateCars)
            {
                var isAvailable = await _rental.IsCarAvailableAsync(
                    car.CarId,
                    pickupDateTime,
                    returnDateTime);

                if (isAvailable)
                {
                    availableCars.Add(car);
                }
            }

            var model = new HomeViewModel
            {
                About = (await _about.TGetListAsync())
                    .FirstOrDefault(x => x.Status),

                Features = (await _feature.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                WhyChooseUs = (await _whyChooseUs.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Processes = (await _process.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Testimonials = (await _testimonial.TGetListAsync())
    .Where(x => x.Status && x.IsApproved)
    .ToList(),

                Faqs = (await _faq.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                OurFutures = (await _ourFuture.TGetListAsync())
                    .Where(x => x.Status)
                    .OrderBy(x => x.Order)
                    .ToList(),

                Awards = (await _award.TGetListAsync())
                    .Where(x => x.Status)
                    .ToList(),

                Cars = availableCars,

                Brands = await _brand.TGetListAsync(),

                Models = await _carModel.TGetListAsync(),

                Branches = await _branch.TGetListAsync(),

                Categories = await _category.TGetListAsync()
            };

            return View("SearchResults", model);
        }
        public async Task<IActionResult> TestCars()
        {
            var cars = await _car.TGetAllCarsWithCategoryAsync();

            return Content(
                string.Join(
                    "\n",
                    cars.Select(c =>
                        $"CarId: {c.CarId} | " +
                        $"Plaka: {c.PlateNumber} | " +
                        $"BrandId: {c.BrandId} | " +
                        $"ModelId: {c.ModelId} | " +
                        $"CategoryId: {c.CategoryId} | " +
                        $"BranchId: {c.BranchId} | " +
                        $"IsActive: {c.IsActive} | " +
                        $"IsAvailable: {c.IsAvailable}"
                    )
                )
            );
        }
        public async Task<IActionResult> TestBranches()
        {
            var branches = await _branch.TGetListAsync();

            return Content(
                string.Join(
                    "\n",
                    branches.Select(b =>
                        $"BranchId: {b.BranchId} | " +
                        $"BranchName: {b.BranchName} | " +
                        $"City: {b.City}"
                    )
                )
            );
        }

        public async Task<IActionResult> TestBrandsModels()
        {
            var brands = await _brand.TGetListAsync();
            var models = await _carModel.TGetListAsync();

            var result = "MARKALAR\n\n";

            result += string.Join(
                "\n",
                brands.Select(x =>
                    $"BrandId: {x.BrandId} | BrandName: {x.BrandName}"
                )
            );

            result += "\n\nMODELLER\n\n";

            result += string.Join(
                "\n",
                models.Select(x =>
                    $"CarModelId: {x.CarModelId} | ModelName: {x.ModelName}"
                )
            );

            return Content(result);
        }


        [Route("Home/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("~/Views/Shared/404.cshtml");
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id
                                ?? HttpContext.TraceIdentifier
                });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTestimonial(
    string Name,
    string? Job,
    int Rating,
    string Comment)
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Comment) ||
                Name.Length > 100 ||
                (Job?.Length ?? 0) > 100 ||
                Comment.Length < 10 ||
                Comment.Length > 1000 ||
                Rating < 1 ||
                Rating > 5)
            {
                TempData["ReviewError"] =
                    "Lütfen bilgileri kontrol edip tekrar deneyin.";

                return RedirectToAction(nameof(Index));
            }

            var testimonial = new Rentaly.EntityLayer.Entities.Testimonial
            {
                Name = Name.Trim(),
                Job = string.IsNullOrWhiteSpace(Job) ? "" : Job.Trim(),
                Comment = Comment.Trim(),
                ImageUrl = "",
                Rating = Rating,
                Status = false
            };

            await _testimonial.TAddAsync(testimonial);

            TempData["ReviewSuccess"] =
                "Yorumunuz alýndý. Yönetici onayýndan sonra yayýmlanacaktýr.";

            return RedirectToAction(nameof(Index));
        }
    }
}