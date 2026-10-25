using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfFeatureDal : GenericRepository<Feature>, IFeatureDal
    {
        public EfFeatureDal(RentalyContext context) : base(context)
        {
        }
    }
}