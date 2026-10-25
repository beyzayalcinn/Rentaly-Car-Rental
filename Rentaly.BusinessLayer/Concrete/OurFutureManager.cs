using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class OurFutureManager : IOurFutureService
    {
        private readonly IOurFutureDal _ourFutureDal;

        public OurFutureManager(IOurFutureDal ourFutureDal)
        {
            _ourFutureDal = ourFutureDal;
        }

        public async Task<List<OurFuture>> TGetListAsync()
        {
            return await _ourFutureDal.GetListAsync();
        }

        public async Task<OurFuture> TGetByIdAsync(int id)
        {
            return await _ourFutureDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(OurFuture entity)
        {
            await _ourFutureDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(OurFuture entity)
        {
            await _ourFutureDal.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _ourFutureDal.DeleteAsync(id);
        }
    }
}