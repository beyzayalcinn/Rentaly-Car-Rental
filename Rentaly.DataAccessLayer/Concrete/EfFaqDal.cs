using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfFaqDal : GenericRepository<Faq>, IFaqDal
    {
        public EfFaqDal(RentalyContext context) : base(context)
        {
        }
    }
}