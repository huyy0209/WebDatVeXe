using System.Linq.Expressions;

namespace WebDatVeXe.Repositories;

// Repository generic: gom cac thao tac CRUD co ban dung chung cho moi entity.
public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> Query();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync();
}
