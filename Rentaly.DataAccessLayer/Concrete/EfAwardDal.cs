using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfAwardDal : GenericRepository<Award>, IAwardDal
    {
        public EfAwardDal(RentalyContext context)
            : base(context)
        {
        }
    }
}