using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarDal:IGenericDal<Car>
    {
       Task<List<Car>> GetAllCarsWithCategoryAsync();
    }
}
