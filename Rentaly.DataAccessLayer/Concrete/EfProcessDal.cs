using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class EfProcessDal : GenericRepository<Process>, IProcessDal
    {
        public EfProcessDal(RentalyContext context) : base(context)
        {
        }
    }
}