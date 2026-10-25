using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Context;

namespace Rentaly.DataAccessLayer.EntityFrameWork;

public class EfCarDal : GenericRepository<Car>, ICarDal
{
    private readonly RentalyContext _context;

    public EfCarDal(RentalyContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Car>> GetAllCarsWithCategoryAsync()
    {
        return await _context.Cars
            .Include(x => x.Category)
            .ToListAsync();
    }
}

