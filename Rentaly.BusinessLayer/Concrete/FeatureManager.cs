using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class FeatureManager : IFeatureService
    {
        private readonly IFeatureDal _featureDal;

        public FeatureManager(IFeatureDal featureDal)
        {
            _featureDal = featureDal;
        }

        public async Task<List<Feature>> TGetListAsync()
        {
            return await _featureDal.GetListAsync();
        }

        public async Task<Feature> TGetByIdAsync(int id)
        {
            return await _featureDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Feature entity)
        {
            await _featureDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Feature entity)
        {
            await _featureDal.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _featureDal.DeleteAsync(id);
        }
    }
}