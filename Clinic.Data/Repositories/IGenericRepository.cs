using Clinic.Data.Entities;

namespace Clinic.Data.Repositories
{
    public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetEntityById(int id);
        Task Create(TEntity entity);
        Task CreateRangeEntities(List<TEntity> entities);
        void Update(TEntity entity);
        Task Delete(int id);
        void DeleteRangeEntities(List<TEntity> entities);
        Task DeletePermanent(int id);
        Task SaveChanges();
    }
}
