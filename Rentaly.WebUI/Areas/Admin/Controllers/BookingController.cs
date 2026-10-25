using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IRentalService _rentalService;

        public BookingController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalService.TGetListAsync();

            return View(rentals);
        }
    }
}