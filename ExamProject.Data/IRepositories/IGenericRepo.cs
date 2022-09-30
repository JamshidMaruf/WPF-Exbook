using ExamProject.Domain.Commons;
using System.Linq.Expressions;

namespace ExamProject.Data.IRepositories
{
    public interface IGenericRepo<T> where T : Auditable
    {
        Task<T> CreateAsync(T entity);
        T UpdateAsync(T entity);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression = null);
        Task SaveChangesAsync();
    }
}
