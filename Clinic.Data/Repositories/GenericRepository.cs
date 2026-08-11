using Clinic.Data.Context;
using Clinic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity> GetEntityById(int id)
        {
            var data = await GetAll().SingleAsync(d => d.Id == id);
            return data;
        }

        public async Task Create(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task CreateRangeEntities(List<TEntity> entities)
        {
            var data = new List<TEntity>();

            foreach (var item in entities)
            {
                item.CreateDate = DateTime.Now;
                item.UpdateDate = DateTime.Now;
                data.Add(item);
            }

            await _dbSet.AddRangeAsync(data);
        }

        public void Update(TEntity entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public async Task Delete(int id)
        {
            var data = await GetEntityById(id);
            data.IsDelete = true;
            Update(data);
        }

        public void DeleteRangeEntities(List<TEntity> entities)
        {
            var data = new List<TEntity>();

            foreach (var item in entities)
            {
                item.IsDelete = true;
                item.UpdateDate = DateTime.Now;
                data.Add(item);
            }

            _dbSet.UpdateRange(data);
        }

        public async Task DeletePermanent(int id)
        {
            var data = await GetEntityById(id);
            _dbSet.Remove(data);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
