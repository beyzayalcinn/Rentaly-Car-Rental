using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CustomerDtos;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;
        private readonly IRentalService _rentalService;
        private readonly ICustomerService _customerService;

        public BookingController(
            ICarService carService,
            IBranchService branchService,
            IRentalService rentalService,
            ICustomerService customerService)
        {
            _carService = carService;
            _branchService = branchService;
            _rentalService = rentalService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int? carId,
            int? pickupBranchId,
            int? returnBranchId,
            DateTime? pickupDate,
            DateTime? returnDate)
        {
            if (!carId.HasValue)
            {
                return RedirectToAction("CarList", "Car");
            }

            var car = await _carService.TGetByIdAsync(carId.Value);

            if (car == null)
            {
                return NotFound();
            }

            await LoadBookingData();

            var model = new BookingViewModel
            {
                CarId = car.CarId,

                PickupBranchId = pickupBranchId ?? car.BranchId,

                ReturnBranchId = returnBranchId ?? car.BranchId,

                PickupDate = pickupDate ??
                             DateTime.Today.AddDays(1).AddHours(10),

                ReturnDate = returnDate ??
                             DateTime.Today.AddDays(2).AddHours(10),

                DrivingLicenseDate = DateTime.Today
            };

            ViewBag.Car = car;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            var car = await _carService.TGetByIdAsync(model.CarId);

            if (car == null)
            {
                return NotFound();
            }

            if (model.PickupDate < DateTime.Now)
            {
                ModelState.AddModelError(
                    nameof(model.PickupDate),
                    "Alış tarihi geçmiş bir tarih olamaz.");
            }

            if (model.ReturnDate <= model.PickupDate)
            {
                ModelState.AddModelError(
                    nameof(model.ReturnDate),
                    "İade tarihi alış tarihinden sonra olmalıdır.");
            }

            if (ModelState.IsValid)
            {
                var isAvailable =
                    await _rentalService.IsCarAvailableAsync(
                        model.CarId,
                        model.PickupDate,
                        model.ReturnDate);

                if (!isAvailable)
                {
                    ModelState.AddModelError(
                        "",
                        "Bu araç seçtiğiniz tarihler arasında müsait değildir. " +
                        "Lütfen farklı bir tarih veya başka bir araç seçiniz.");
                }
            }
            if (!ModelState.IsValid)
            {
                await LoadBookingData();

                ViewBag.Car = car;

                return View(model);
            }

            var customerDto = new CreateCustomerDto
            {
                Name = model.Name.Trim(),
                SurName = model.SurName.Trim(),
                Email = model.Email.Trim(),
                Phone = model.Phone.Trim(),
                IdentityNumber = model.IdentityNumber.Trim(),
                DrivingLicenseNumber =
                    model.DrivingLicenseNumber.Trim(),
                DrivingLicenseDate = model.DrivingLicenseDate
            };

            var customerId =
                await _customerService.TInsertAndGetIdAsync(
                    customerDto);

            var totalDays =
                (model.ReturnDate.Date -
                 model.PickupDate.Date).Days;

            if (totalDays < 1)
            {
                totalDays = 1;
            }

            var totalPrice =
                totalDays * car.DailyPrice;

            var rental = new Rental
            {
                CarId = model.CarId,

                CustomerId = customerId,

                PickupBranchId =
                    model.PickupBranchId,

                ReturnBranchId =
                    model.ReturnBranchId,

                PickupDate =
                    model.PickupDate,

                ReturnDate =
                    model.ReturnDate,

                TotalPrice =
                    totalPrice,

                Status = "Pending"
            };

            await _rentalService.TInsertAsync(rental);

            TempData["BookingSuccess"] =
                "Rezervasyonunuz başarıyla oluşturuldu. " +
                "Rezervasyonunuz admin onayı bekliyor.";

            return RedirectToAction(
                "Success",
                "Booking",
                new
                {
                    rentalId = rental.RentalId
                });
        }
        [HttpGet]
        public async Task<IActionResult> Success(int rentalId)
        {
            var rental =
                await _rentalService.TGetByIdAsync(rentalId);

            if (rental == null)
            {
                return NotFound();
            }

            var car =
                await _carService.TGetByIdAsync(
                    rental.CarId);

            ViewBag.Car = car;

            return View(rental);
        }
        private async Task LoadBookingData()
        {
            var branches =
                await _branchService.TGetListAsync();

            ViewBag.Branches =
                new SelectList(
                    branches,
                    "BranchId",
                    "BranchName");
        }
    }
}