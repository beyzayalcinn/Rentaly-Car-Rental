
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IBranchService _branchService;
        private readonly IEmailService _emailService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;

        public RentalController(
            IRentalService rentalService,
            ICarService carService,
            ICustomerService customerService,
            IBranchService branchService,
            IEmailService emailService,
            IBrandService brandService,
            ICarModelService carModelService)
        {
            _rentalService = rentalService;
            _carService = carService;
            _customerService = customerService;
            _branchService = branchService;
            _emailService = emailService;
            _brandService = brandService;
            _carModelService = carModelService;
        }


        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalService.TGetListAsync();

            var cars = await _carService.TGetListAsync();
            var customers = await _customerService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();

            ViewBag.Cars = cars;
            ViewBag.Customers = customers;
            ViewBag.Branches = branches;

            return View(rentals);
        }


        public async Task<IActionResult> Detail(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            var car = await _carService.TGetByIdAsync(rental.CarId);
            var customer = await _customerService.TGetByIdAsync(rental.CustomerId);
            var branches = await _branchService.TGetListAsync();

            var pickupBranch = branches
                .FirstOrDefault(x => x.BranchId == rental.PickupBranchId);

            var returnBranch = branches
                .FirstOrDefault(x => x.BranchId == rental.ReturnBranchId);

            ViewBag.Car = car;
            ViewBag.Customer = customer;
            ViewBag.PickupBranch = pickupBranch;
            ViewBag.ReturnBranch = returnBranch;

            return View(rental);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rental rental)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();

                return View(rental);
            }

            if (rental.ReturnDate <= rental.PickupDate)
            {
                ModelState.AddModelError(
                    "ReturnDate",
                    "Teslim tarihi alış tarihinden sonra olmalıdır.");

                await LoadDropdowns();

                return View(rental);
            }

            var isAvailable =
                await _rentalService.IsCarAvailableAsync(
                    rental.CarId,
                    rental.PickupDate,
                    rental.ReturnDate);

            if (!isAvailable)
            {
                ModelState.AddModelError(
                    "CarId",
                    "Seçilen araç bu tarihler arasında müsait değil.");

                await LoadDropdowns();

                return View(rental);
            }

            rental.Status = "Pending";

            await _rentalService.TInsertAsync(rental);

            TempData["Success"] =
                "Rezervasyon başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            await LoadDropdowns();

            return View(rental);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Rental rental)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();

                return View(rental);
            }

            if (rental.ReturnDate <= rental.PickupDate)
            {
                ModelState.AddModelError(
                    "ReturnDate",
                    "Teslim tarihi alış tarihinden sonra olmalıdır.");

                await LoadDropdowns();

                return View(rental);
            }

            var isAvailable =
                await _rentalService.IsCarAvailableAsync(
                    rental.CarId,
                    rental.PickupDate,
                    rental.ReturnDate,
                    rental.RentalId);

            if (!isAvailable)
            {
                ModelState.AddModelError(
                    "CarId",
                    "Seçilen araç bu tarihler arasında başka bir rezervasyonda.");

                await LoadDropdowns();

                return View(rental);
            }

            await _rentalService.TUpdateAsync(rental);

            TempData["Success"] =
                "Rezervasyon başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // REZERVASYON ONAYLAMA + EMAIL GÖNDERME
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.Status == "Approved")
            {
                TempData["Error"] =
                    "Bu rezervasyon zaten onaylanmış.";

                return RedirectToAction(nameof(Index));
            }

            if (rental.Status == "Rejected")
            {
                TempData["Error"] =
                    "Reddedilmiş bir rezervasyon onaylanamaz.";

                return RedirectToAction(nameof(Index));
            }

            if (rental.Status == "Cancelled")
            {
                TempData["Error"] =
                    "İptal edilmiş bir rezervasyon onaylanamaz.";

                return RedirectToAction(nameof(Index));
            }


            // =========================================================
            // ARAÇ MÜSAİTLİK KONTROLÜ
            // =========================================================

            var isAvailable =
                await _rentalService.IsCarAvailableAsync(
                    rental.CarId,
                    rental.PickupDate,
                    rental.ReturnDate,
                    rental.RentalId);

            if (!isAvailable)
            {
                TempData["Error"] =
                    "Bu araç seçilen tarihler arasında başka bir rezervasyon tarafından kullanılıyor.";

                return RedirectToAction(nameof(Index));
            }


            // =========================================================
            // MÜŞTERİ
            // =========================================================

            var customer =
                await _customerService.TGetByIdAsync(
                    rental.CustomerId);

            if (customer == null)
            {
                TempData["Error"] =
                    "Rezervasyona ait müşteri bulunamadı.";

                return RedirectToAction(nameof(Index));
            }


            // =========================================================
            // EMAIL KONTROLÜ
            // =========================================================

            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                TempData["Error"] =
                    "Müşterinin email adresi bulunamadı.";

                return RedirectToAction(nameof(Index));
            }


            // =========================================================
            // ARAÇ
            // =========================================================

            var car =
                await _carService.TGetByIdAsync(
                    rental.CarId);

            if (car == null)
            {
                TempData["Error"] =
                    "Rezervasyona ait araç bulunamadı.";

                return RedirectToAction(nameof(Index));
            }


            // =========================================================
            // MARKA
            // =========================================================

            var brand =
                await _brandService.TGetByIdAsync(
                    car.BrandId);


            // =========================================================
            // MODEL
            // =========================================================

            var carModel =
                await _carModelService.TGetByIdAsync(
                    car.ModelId);


            string brandName =
                brand?.BrandName ?? "Belirtilmemiş";

            string modelName =
                carModel?.ModelName ?? "Belirtilmemiş";


            // =========================================================
            // REZERVASYONU ONAYLA
            // =========================================================

            rental.Status = "Approved";

            await _rentalService.TUpdateAsync(rental);


            // =========================================================
            // EMAIL BİLGİLERİ
            // =========================================================

            var subject =
                "Rentaly - Rezervasyonunuz Onaylandı";

            var reservationNumber =
                $"REC{rental.RentalId}";

            // Bir sonraki rezervasyonda kullanılacak indirim kodu
            var couponCode = "RENTAOBC";


            // =========================================================
            // HTML EMAIL
            // =========================================================

            var body = $@"
<!DOCTYPE html>

<html lang='tr'>

<head>

    <meta charset='UTF-8'>

    <meta name='viewport'
          content='width=device-width, initial-scale=1.0'>

    <title>Rentaly - Rezervasyon Onayı</title>

    <style>

        body {{
            margin: 0;
            padding: 0;
            background-color: #f4f6f8;
            font-family: Arial, Helvetica, sans-serif;
            color: #333333;
        }}

        .container {{
            width: 100%;
            max-width: 650px;
            margin: 30px auto;
            background-color: #ffffff;
            border-radius: 14px;
            overflow: hidden;
            box-shadow: 0 5px 20px rgba(0,0,0,0.08);
        }}

        .header {{
            background-color: #111111;
            padding: 32px 20px;
            text-align: center;
        }}

        .header-logo {{
            color: #ffffff;
            font-size: 30px;
            font-weight: bold;
            letter-spacing: 4px;
            margin: 0;
        }}

        .header-text {{
            color: #cccccc;
            font-size: 14px;
            margin-top: 8px;
        }}

        .content {{
            padding: 35px;
        }}

        .success {{
            text-align: center;
            color: #28a745;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 25px;
        }}

        .hello {{
            font-size: 17px;
            margin-bottom: 15px;
        }}

        .car-box {{
            margin-top: 25px;
            padding: 25px;
            background-color: #111111;
            color: #ffffff;
            border-radius: 10px;
            text-align: center;
        }}

        .car-name {{
            font-size: 25px;
            font-weight: bold;
            margin-bottom: 10px;
        }}

        .plate {{
            font-size: 16px;
            color: #dddddd;
        }}

        .info-box {{
            background-color: #f8f9fa;
            border-radius: 10px;
            padding: 22px;
            margin-top: 25px;
        }}

        .info-title {{
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #111111;
        }}

        .info-row {{
            padding: 11px 0;
            border-bottom: 1px solid #e5e5e5;
            font-size: 14px;
        }}

        .info-row:last-child {{
            border-bottom: none;
        }}

        .label {{
            font-weight: bold;
            color: #222222;
        }}

        /* =====================================================
           İNDİRİM KARTI
           ===================================================== */

        .coupon {{
            margin-top: 30px;
            padding: 30px 20px;
            background-color: #111111;
            border-radius: 14px;
            text-align: center;
        }}

        .coupon-brand {{
            color: #bbbbbb;
            font-size: 13px;
            letter-spacing: 4px;
            font-weight: bold;
            margin-bottom: 12px;
        }}

        .coupon-title {{
            color: #ffffff;
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 10px;
        }}

        .discount {{
            color: #ffffff;
            font-size: 42px;
            font-weight: bold;
            margin: 8px 0;
        }}

        .coupon-description {{
            color: #cccccc;
            font-size: 14px;
            line-height: 1.6;
            margin: 12px 0 22px 0;
        }}

        .coupon-code {{
            display: inline-block;
            background-color: #ffffff;
            color: #111111;
            padding: 14px 28px;
            border-radius: 7px;
            font-size: 19px;
            font-weight: bold;
            letter-spacing: 3px;
        }}

        .coupon-note {{
            color: #999999;
            font-size: 11px;
            margin-top: 15px;
        }}

        .thank-you {{
            margin-top: 30px;
            line-height: 1.7;
        }}

        /* =====================================================
           İMZA
           ===================================================== */

        .signature {{
            margin-top: 35px;
            padding-top: 25px;
            border-top: 1px solid #dddddd;
        }}

        .signature-name {{
            font-size: 20px;
            font-weight: bold;
            color: #111111;
            margin-bottom: 8px;
        }}

        .signature-info {{
            color: #666666;
            font-size: 13px;
            line-height: 1.8;
        }}

        .footer {{
            background-color: #f8f9fa;
            padding: 25px 20px;
            text-align: center;
            color: #777777;
            font-size: 12px;
            line-height: 1.6;
        }}

    </style>

</head>


<body>

<div class='container'>


    <!-- =====================================================
         HEADER
         ===================================================== -->

    <div class='header'>

        <div class='header-logo'>
            RENTALY
        </div>

        <div class='header-text'>
            Premium Araç Kiralama
        </div>

    </div>


    <!-- =====================================================
         CONTENT
         ===================================================== -->

    <div class='content'>


        <!-- BAŞARI MESAJI -->

        <div class='success'>

            🎉 Rezervasyonunuz Onaylandı!

        </div>


        <!-- MÜŞTERİ -->

        <p class='hello'>

            Merhaba
            <strong>
                {customer.Name} {customer.SurName}
            </strong>,

        </p>


        <p>

            Rentaly üzerinden oluşturduğunuz
            araç rezervasyonunuz sistemimiz tarafından
            başarıyla onaylanmıştır.

        </p>


        <!-- =================================================
             ARAÇ BİLGİLERİ
             ================================================= -->

        <div class='car-box'>

            <div class='car-name'>

                {brandName} {modelName}

            </div>

            <div class='plate'>

                Plaka:
                <strong>{car.PlateNumber}</strong>

            </div>

        </div>


        <!-- =================================================
             REZERVASYON BİLGİLERİ
             ================================================= -->

        <div class='info-box'>

            <div class='info-title'>

                🚗 Rezervasyon Bilgileri

            </div>


            <div class='info-row'>

                <span class='label'>
                    Rezervasyon No:
                </span>

                #{reservationNumber}

            </div>


            <div class='info-row'>

                <span class='label'>
                    Alış Tarihi:
                </span>

                {rental.PickupDate:dd.MM.yyyy HH:mm}

            </div>


            <div class='info-row'>

                <span class='label'>
                    İade Tarihi:
                </span>

                {rental.ReturnDate:dd.MM.yyyy HH:mm}

            </div>


            <div class='info-row'>

                <span class='label'>
                    Toplam Tutar:
                </span>

                {rental.TotalPrice:N2} ₺

            </div>

        </div>


        <!-- =================================================
             GÖRSEL İNDİRİM KUPONU
             ================================================= -->

        <div class='coupon'>

            <div class='coupon-brand'>

                RENTALY

            </div>


            <div class='coupon-title'>

                SİZE ÖZEL FIRSAT

            </div>


            <div class='discount'>

                %15 İNDİRİM

            </div>


            <div class='coupon-description'>

                Bir sonraki araç rezervasyonunuzda
                kullanabileceğiniz özel indirim kodunuz.

            </div>


            <div class='coupon-code'>

                {couponCode}

            </div>


            <div class='coupon-note'>

                Bu kod bir sonraki rezervasyonunuzda kullanılabilir.

            </div>

        </div>


        <!-- =================================================
             TEŞEKKÜR
             ================================================= -->

        <div class='thank-you'>

            <p>

                Rentaly'yi tercih ettiğiniz için teşekkür ederiz.

            </p>

            <p>

                Size güvenli ve keyifli yolculuklar dileriz! 🚗

            </p>

        </div>


        <!-- =================================================
             RENTALY İMZA
             ================================================= -->

        <div class='signature'>

            <div class='signature-name'>

                Rentaly Araç Kiralama

            </div>

            <div class='signature-info'>

                Müşteri Hizmetleri<br>

                +90 555 555 55 55<br>

                contact@rentaly.com

            </div>

        </div>


    </div>


    <!-- =====================================================
         FOOTER
         ===================================================== -->

    <div class='footer'>

        Bu email Rentaly rezervasyon sisteminiz
        tarafından otomatik olarak gönderilmiştir.<br>

        Rentaly Araç Kiralama

    </div>


</div>

</body>

</html>
";


            // =========================================================
            // EMAIL GÖNDER
            // =========================================================

            await _emailService.SendEmailAsync(
                customer.Email,
                subject,
                body);


            TempData["Success"] =
                "Rezervasyon başarıyla onaylandı ve müşteriye email gönderildi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.Status == "Approved")
            {
                TempData["Error"] =
                    "Onaylanmış bir rezervasyon reddedilemez.";

                return RedirectToAction(nameof(Index));
            }

            if (rental.Status == "Rejected")
            {
                TempData["Error"] =
                    "Bu rezervasyon zaten reddedilmiş.";

                return RedirectToAction(nameof(Index));
            }

            rental.Status = "Rejected";

            await _rentalService.TUpdateAsync(rental);

            TempData["Success"] =
                "Rezervasyon reddedildi.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _rentalService.TGetByIdAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            await _rentalService.TDeleteAsync(id);

            TempData["Success"] =
                "Rezervasyon silindi.";

            return RedirectToAction(nameof(Index));
        }


        private async Task LoadDropdowns()
        {
            var cars = await _carService.TGetListAsync();
            var customers = await _customerService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();

            ViewBag.Cars = new SelectList(
                cars,
                "CarId",
                "CarId");

            ViewBag.Customers = new SelectList(
                customers,
                "CustomerId",
                "CustomerId");

            ViewBag.Branches = new SelectList(
                branches,
                "BranchId",
                "BranchName");
        }
    }
}

