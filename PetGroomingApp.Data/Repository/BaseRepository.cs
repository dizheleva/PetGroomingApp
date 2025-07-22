namespace PetGroomingApp.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class BaseRepository<TType, TKey> : IRepository<TType, TKey>
        where TType : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TType> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TType>();
        }

        public TType? GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TType?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public TType? FirstOrDefault(Func<TType, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public async Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TType> GetAll()
        {
            return _dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await _dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(TType item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public void AddRange(TType[] items)
        {
            _dbSet.AddRange(items);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(TType[] items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public bool Delete(TType entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TType entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool HardDelete(TType entity)
        {
            _dbSet.Remove(entity);

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> HardDeleteAsync(TType entity)
        {
            _dbSet.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(TType item)
        {
            try
            {
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TType?> FindByConditionsAsync(Expression<Func<TType, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
