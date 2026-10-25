using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class WhyChooseUsManager : IWhyChooseUsService
    {
        private readonly IWhyChooseUsDal _whyChooseUsDal;

        public WhyChooseUsManager(IWhyChooseUsDal whyChooseUsDal)
        {
            _whyChooseUsDal = whyChooseUsDal;
        }

        public async Task<List<WhyChooseUs>> TGetListAsync()
        {
            return await _whyChooseUsDal.GetListAsync();
        }

        public async Task<WhyChooseUs> TGetByIdAsync(int id)
        {
            return await _whyChooseUsDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(WhyChooseUs entity)
        {
            await _whyChooseUsDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(WhyChooseUs entity)
        {
            await _whyChooseUsDal.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _whyChooseUsDal.DeleteAsync(id);
        }
    }
}