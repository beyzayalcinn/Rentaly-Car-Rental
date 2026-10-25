using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class FaqManager : IFaqService
    {
        private readonly IFaqDal _faqDal;

        public FaqManager(IFaqDal faqDal)
        {
            _faqDal = faqDal;
        }

        public async Task<List<Faq>> TGetListAsync()
        {
            return await _faqDal.GetListAsync();
        }

        public async Task<Faq> TGetByIdAsync(int id)
        {
            return await _faqDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Faq entity)
        {
            await _faqDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Faq entity)
        {
            await _faqDal.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _faqDal.DeleteAsync(id);
        }
    }
}