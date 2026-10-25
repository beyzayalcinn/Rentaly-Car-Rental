using Rentaly.DtoLayer.CustomerDtos;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> TGetListAsync();

        Task<GetCustomerByIdDto> TGetByIdAsync(int id);

        Task TInsertAsync(CreateCustomerDto dto);

        Task<int> TInsertAndGetIdAsync(CreateCustomerDto dto);

        Task TUpdateAsync(UpdateCustomerDto dto);

        Task TDeleteAsync(int id);
    }
}