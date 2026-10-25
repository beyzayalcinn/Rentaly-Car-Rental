using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Rentaly.DataAccessLayer.Context;
namespace Rentaly.DataAccessLayer.EntityFrameWork
{
    public class EfCustomerDal : GenericRepository<Customer>, ICustomerDal
    {
        public EfCustomerDal(RentalyContext context) : base(context)
        {
        }
    }
}
