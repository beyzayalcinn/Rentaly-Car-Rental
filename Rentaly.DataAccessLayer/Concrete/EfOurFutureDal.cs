using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfOurFutureDal : GenericRepository<OurFuture>, IOurFutureDal
    {
        public EfOurFutureDal(RentalyContext context)
            : base(context)
        {
        }
    }
}