using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IRentalDal : IGenericDal<Rental>
    {
        Task<bool> IsCarAvailableAsync(
            int carId,
            DateTime pickupDate,
            DateTime returnDate,
            int? excludedRentalId = null);
    }
}