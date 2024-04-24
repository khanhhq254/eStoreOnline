using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline.Infrastructure.Abstractions;

public class BaseRepository<TModel> : IRepository<TModel> where TModel : class
{
    private readonly DbContext _context;
    private readonly DbSet<TModel> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
    }

    public void AddAsync(TModel model)
    {
        _dbSet.Add(model);
    }

    public void UpdateAsync(TModel model)
    {
        _dbSet.Update(model);
    }

    public void DeleteAsync(TModel model)
    {
        _dbSet.Remove(model);
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<TModel> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<TModel> AsQueryable(Expression<Func<TModel, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
}