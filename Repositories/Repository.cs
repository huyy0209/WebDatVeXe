using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;

namespace WebDatVeXe.Repositories;

// Cai dat generic repository tren EF Core.
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<List<T>> GetAllAsync() => await _set.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

    public IQueryable<T> Query() => _set.AsQueryable();

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _set.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity);

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();
}
