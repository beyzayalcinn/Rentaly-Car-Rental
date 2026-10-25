using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfCarModelDal : GenericRepository<CarModel>, ICarModelDal
    {
        public EfCarModelDal(RentalyContext context)
            : base(context)
        {
        }
    }
}