using Microsoft.EntityFrameworkCore;
using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            Expression<Func<T, bool>>? filter = null
            )
        {
            var entities = _context.Set<T>().AsQueryable();
            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            return await entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(
            Expression<Func<T, bool>>? filter = null
            )
        {
            return (await (GetAllAsync(filter))).FirstOrDefault()!;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database.", ex);
            }
        }
    }
}
