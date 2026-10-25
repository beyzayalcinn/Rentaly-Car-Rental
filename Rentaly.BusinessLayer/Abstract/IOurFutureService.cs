using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IOurFutureService
    {
        Task<List<OurFuture>> TGetListAsync();
        Task<OurFuture> TGetByIdAsync(int id);
        Task TInsertAsync(OurFuture entity);
        Task TUpdateAsync(OurFuture entity);
        Task TDeleteAsync(int id);
    }
}