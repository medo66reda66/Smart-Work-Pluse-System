
using System.Linq.Expressions;

namespace SmartWorkSpaseADL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        );

        Task<T> GetByIdAsync(
            Expression<Func<T, bool>>? filter = null
        );

        Task SaveChangesAsync();
    }
}
