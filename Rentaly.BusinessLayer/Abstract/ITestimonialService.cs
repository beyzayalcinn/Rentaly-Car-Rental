using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ITestimonialService
    {
        Task<List<Testimonial>> TGetListAsync();
        Task<Testimonial> TGetByIdAsync(int id);
        Task TAddAsync(Testimonial testimonial);
        Task TInsertAsync(Testimonial entity);
        Task TUpdateAsync(Testimonial entity);
        Task TDeleteAsync(int id);
    }
}