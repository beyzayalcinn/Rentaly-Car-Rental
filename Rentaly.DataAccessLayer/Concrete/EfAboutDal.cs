using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfAboutDal : GenericRepository<About>, IAboutDal
    {
        public EfAboutDal(RentalyContext context) : base(context)
        {
        }
    }
}