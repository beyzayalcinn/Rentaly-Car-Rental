using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IFeatureService
    {
        Task<List<Feature>> TGetListAsync();
        Task<Feature> TGetByIdAsync(int id);
        Task TInsertAsync(Feature entity);
        Task TUpdateAsync(Feature entity);
        Task TDeleteAsync(int id);
    }
}