using System.Linq.Expressions;

namespace eStoreOnline.Infrastructure.Abstractions;

public interface IRepository<TModel>
{
    void AddAsync(TModel model);
    void UpdateAsync(TModel model);
    void DeleteAsync(TModel model);
    Task<TModel?> GetByIdAsync(int id);
    IQueryable<TModel> AsQueryable();
    IQueryable<TModel> AsQueryable(Expression<Func<TModel, bool>> expression);
}