using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BM.Common.Infrastructure.Repositories.Generic
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
    {
        // Fields to store the DbContext and DbSet for the entity.
        private readonly TContext _context;
        private readonly DbSet<TEntity> _set;

        // Constructor to initialize the repository with a DbContext and DbSet.
        public GenericRepository(TContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        // Adds or updates a single entity asynchronously.
        public async Task AddOrUpdateAsync(TEntity entity)
        {
            _set.Update(entity);

            await _context.SaveChangesAsync();
        }

        // Adds or updates a collection of entities asynchronously.
        public async Task AddOrUpdateAsync(IEnumerable<TEntity> entities)
        {
            _set.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        // Deletes a single entity asynchronously.
        public async Task DeleteAsync(TEntity entity)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Deletes an entity by its ID asynchronously.
        public async Task DeleteAsync(object id)
        {
            var entity = await this.GetAsync(id);
            await this.DeleteAsync(entity);
        }

        // Retrieves a list of entities based on filter, order, and optional includes asynchronously.
        public async Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            bool asNoTracking = false,
            params string[] includeProperties)
        {
            var query = this.GetQuery();

            // Include related entities if specified.
            if (includeProperties is { Length: > 0 })
            {
                query = includeProperties.Aggregate(query, (current, prop) => current.Include(prop));
            }

            // Apply filter if specified.
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering if specified.
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply AsNoTracking if specified.
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        // Retrieves a single entity by its ID asynchronously.
        public async Task<TEntity> GetAsync(object id, bool asNoTracking = false)
        {
            var entity = await _set.FindAsync(id);

            // Detach the entity if AsNoTracking is specified.
            if (asNoTracking)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        // Deletes a collection of entities asynchronously.
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            _set.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        // Retrieves a queryable representation of the DbSet with AsNoTracking applied.
        protected IQueryable<TEntity> GetQuery()
        {
            return _set.AsQueryable().AsNoTracking();
        }
    }
}
