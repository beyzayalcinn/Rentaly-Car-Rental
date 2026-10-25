using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfWhyChooseUsDal : GenericRepository<WhyChooseUs>, IWhyChooseUsDal
    {
        public EfWhyChooseUsDal(RentalyContext context) : base(context)
        {
        }
    }
}