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
        private DbSet<T> _table;

        public CommonRepository(DataContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetDetails(int id)
        {
            return _table.Find(id);
        }

        public void Insert(T item)
        {
            _table.Add(item);
        }

        public void Update(T item)
        {
            _table.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(T item)
        {
            _table.Remove(item);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }       
    }
} 