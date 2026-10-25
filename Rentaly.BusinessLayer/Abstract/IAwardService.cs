using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IAwardService
    {
        Task<List<Award>> TGetListAsync();
        Task<Award> TGetByIdAsync(int id);
        Task TInsertAsync(Award entity);
        Task TUpdateAsync(Award entity);
        Task TDeleteAsync(int id);
    }
}