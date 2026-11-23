
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SmartWorkSpaseADL.Repositories.Classes
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Expression<Func<T, object>>[]? includes = null
            )
        {
            var entities = _context.Set<T>().AsQueryable();

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            if (orderBy != null)
            {
                entities = orderBy(entities);
            }

            if (includes != null)
            {
                foreach (var enclude in includes)
                {
                    entities = entities.Include(enclude);
                }
            }

            return await entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Expression<Func<T, object>>[]? includes = null
            )
        {
            return (await GetAllAsync(filter, orderBy,includes)).FirstOrDefault()!;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(
                    "An error occurred while saving changes to the database.",
                    ex
                );
            }
        }
    }
}
