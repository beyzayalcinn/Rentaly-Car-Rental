using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalDal : GenericRepository<Rental>, IRentalDal
    {
        private readonly RentalyContext _context;

        public RentalDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsCarAvailableAsync(
            int carId,
            DateTime pickupDate,
            DateTime returnDate,
            int? excludedRentalId = null)
        {
            var query = _context.Rentals
                .Where(x =>
                    x.CarId == carId &&
                    x.Status == "Approved" &&
                    x.PickupDate < returnDate &&
                    x.ReturnDate > pickupDate);

            if (excludedRentalId.HasValue)
            {
                query = query.Where(
                    x => x.RentalId != excludedRentalId.Value);
            }

            var isRented = await query.AnyAsync();

            return !isRented;
        }
    }
}