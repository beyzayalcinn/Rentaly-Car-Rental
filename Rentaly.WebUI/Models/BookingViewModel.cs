using System.ComponentModel.DataAnnotations;

namespace Rentaly.WebUI.Models
{
    public class BookingViewModel
    {
        public int CarId { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
        [StringLength(11, MinimumLength = 11,
            ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
        [Display(Name = "TC Kimlik No")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Ehliyet numarası zorunludur.")]
        [Display(Name = "Ehliyet Numarası")]
        public string DrivingLicenseNumber { get; set; }

        [Required(ErrorMessage = "Ehliyet tarihi zorunludur.")]
        [Display(Name = "Ehliyet Tarihi")]
        public DateTime DrivingLicenseDate { get; set; }

        [Required(ErrorMessage = "Alış şubesi seçiniz.")]
        [Display(Name = "Alış Şubesi")]
        public int PickupBranchId { get; set; }

        [Required(ErrorMessage = "Teslim şubesi seçiniz.")]
        [Display(Name = "Teslim Şubesi")]
        public int ReturnBranchId { get; set; }

        [Required(ErrorMessage = "Alış tarihi seçiniz.")]
        [Display(Name = "Alış Tarihi")]
        public DateTime PickupDate { get; set; }

        [Required(ErrorMessage = "Teslim tarihi seçiniz.")]
        [Display(Name = "Teslim Tarihi")]
        public DateTime ReturnDate { get; set; }
    }
}