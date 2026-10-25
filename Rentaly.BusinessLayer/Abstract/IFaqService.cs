using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IFaqService
    {
        Task<List<Faq>> TGetListAsync();
        Task<Faq> TGetByIdAsync(int id);
        Task TInsertAsync(Faq entity);
        Task TUpdateAsync(Faq entity);
        Task TDeleteAsync(int id);
    }
}