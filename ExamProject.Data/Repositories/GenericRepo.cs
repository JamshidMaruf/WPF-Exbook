using ExamProject.Data.Contexts;
using ExamProject.Data.IRepositories;
using ExamProject.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExamProject.Data.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : Auditable
    {
        private readonly ExamProjectContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepo()
        {
            _context = new ExamProjectContext();
            _dbSet = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
            => (await _dbSet.AddAsync(entity)).Entity;

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            var entity = await GetAsync(expression);

            if (entity is null)
            {
                return false;
            }
            _dbSet.Remove(entity);

            return true;
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> expression)
            => _dbSet.FirstOrDefaultAsync(expression);

        public T UpdateAsync(T entity)
            => _dbSet.Update(entity).Entity;

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression = null)
            => expression is null ? _dbSet : _dbSet.Where(expression);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
