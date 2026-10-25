using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IWhyChooseUsService
    {
        Task<List<WhyChooseUs>> TGetListAsync();
        Task<WhyChooseUs> TGetByIdAsync(int id);
        Task TInsertAsync(WhyChooseUs entity);
        Task TUpdateAsync(WhyChooseUs entity);
        Task TDeleteAsync(int id);
    }
}