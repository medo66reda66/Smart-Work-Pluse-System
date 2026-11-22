using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<List<T>> GetAllAsync(
          Expression<Func<T, bool>>? filter = null
          );

        Task<T> GetByIdAsync(
          Expression<Func<T, bool>>? filter = null
          );

        Task SaveChangesAsync();
       
    }
}

