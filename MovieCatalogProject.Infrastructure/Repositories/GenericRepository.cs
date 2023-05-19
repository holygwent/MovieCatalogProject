using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogProject.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _entities = _context.Set<T>();
            _logger = logger;
        }
        public async Task AddAsync(T entity)
        {

            await _entities.AddAsync(entity);
            SaveChangesAsync().Wait();
        }

        public void Delete(object id)
        {

            var entity = GetByIdAsync(id).Result;
            if (entity == null)
                throw new NotFoundException($"there is no entity with id {id}");

            _logger.LogWarning($"Entity with id:{id} DELETE action invoke");
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            SaveChangesAsync().Wait();

        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var entity = await _entities.FindAsync(id);
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //public void Update(T entity)
        //{
        //    EntityEntry entityEntry = _context.Entry<T>(entity);
        //    entityEntry.State = EntityState.Modified;
        //    SaveChangesAsync().Wait();

        //}
    }
}
