using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfTestimonialDal : GenericRepository<Testimonial>, ITestimonialDal
    {
        public EfTestimonialDal(RentalyContext context) : base(context)
        {
        }
    }
}