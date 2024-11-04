using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApiDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApiDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Add(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null) {
                 _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);   
        }
    }
}
