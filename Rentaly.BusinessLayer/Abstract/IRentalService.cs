using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IRentalService : IGenericService<Rental>
    {
        Task<bool> IsCarAvailableAsync(
            int carId,
            DateTime pickupDate,
            DateTime returnDate,
            int? excludedRentalId = null);
    }
}