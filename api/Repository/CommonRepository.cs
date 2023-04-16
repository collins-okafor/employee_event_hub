using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly DataContext _context;

        public CommonRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetDetails(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Insert(T item)
        {
            _context.Set<T>().Add(item);
            await _context.SaveChangesAsync();
            return item;

        }

        public async Task<T> Update(T item)
        {
            // _context.Set<T>().Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                return entity;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
} 