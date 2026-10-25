using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CustomerDtos;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.TGetListAsync();

            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _customerService.TInsertAsync(dto);

            TempData["Success"] = "Müşteri başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerService.TGetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var customer = await _customerService.TGetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var dto = new UpdateCustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                SurName = customer.SurName,
                Email = customer.Email,
                Phone = customer.Phone,
                IdentityNumber = customer.IdentityNumber,
                DrivingLicenseNumber = customer.DrivingLicenseNumber,
                DrivingLicenseDate = customer.DrivingLicenseDate
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _customerService.TUpdateAsync(dto);

            TempData["Success"] = "Müşteri başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.TGetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.TDeleteAsync(id);

            TempData["Success"] = "Müşteri başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}