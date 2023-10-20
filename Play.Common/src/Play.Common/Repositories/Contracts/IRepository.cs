using System.Linq.Expressions;
using Play.Common.Entities;

namespace Play.Common.Repositories;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter);
    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveAsync(Guid id);
}