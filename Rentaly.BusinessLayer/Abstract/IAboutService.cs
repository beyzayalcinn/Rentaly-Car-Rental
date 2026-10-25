using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IAboutService
    {
        Task<List<About>> TGetListAsync();
        Task<About> TGetByIdAsync(int id);
        Task TInsertAsync(About entity);
        Task TUpdateAsync(About entity);
        Task TDeleteAsync(int id);
    }
}