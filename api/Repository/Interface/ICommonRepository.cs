using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repository.Interface
{
    public interface ICommonRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetDetails(int id);
        Task<T> Insert(T item);
        Task<T> Update(T item);
        Task<T> Delete(int id);
        // int SaveChanges();
    }
}