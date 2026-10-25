using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _rentalDal.DeleteAsync(id);
        }

        public async Task<Rental> TGetByIdAsync(int id)
        {
            return await _rentalDal.GetByIdAsync(id);
        }

        public async Task<List<Rental>> TGetListAsync()
        {
            return await _rentalDal.GetListAsync();
        }

        public async Task TInsertAsync(Rental entity)
        {
            await _rentalDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Rental entity)
        {
            await _rentalDal.UpdateAsync(entity);
        }

        public async Task<bool> IsCarAvailableAsync(
      int carId,
      DateTime pickupDate,
      DateTime returnDate,
      int? excludedRentalId = null)
        {
            return await _rentalDal.IsCarAvailableAsync(
                carId,
                pickupDate,
                returnDate,
                excludedRentalId);
        }
    }
}