using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Rentaly.DataAccessLayer.Context;
namespace Rentaly.DataAccessLayer.EntityFrameWork;
  public class EfBranchDal : GenericRepository<Branch>, IBranchDal
{
    public EfBranchDal(RentalyContext context) : base(context)
    {
    }
}