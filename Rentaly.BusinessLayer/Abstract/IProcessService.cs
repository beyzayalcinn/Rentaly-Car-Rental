using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IProcessService
    {
        Task<List<Process>> TGetListAsync();
        Task<Process> TGetByIdAsync(int id);
        Task TInsertAsync(Process entity);
        Task TUpdateAsync(Process entity);
        Task TDeleteAsync(int id);
    }
}