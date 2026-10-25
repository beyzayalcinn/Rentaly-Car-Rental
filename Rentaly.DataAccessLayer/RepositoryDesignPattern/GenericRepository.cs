using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Context;

namespace Rentaly.DataAccessLayer.RepositoryDesignPattern
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly RentalyContext _context;
        public GenericRepository(RentalyContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(int id)
        {
            var value = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>>GetListAsync()
        {
            return await _context.Set<T>().ToListAsync(); 
        }

        public async Task InsertAsync(T entity)
        {
            var value = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var values = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
