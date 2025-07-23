namespace PetGroomingApp.Data.Repository.Interfaces
{
    using System.Linq.Expressions;

    public interface IRepository<TType, TKey>
    {
        TType? GetById(TKey id);

        Task<TType?> GetByIdAsync(TKey id);

        TType? FirstOrDefault(Func<TType, bool> predicate);

        Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate);

        IEnumerable<TType> GetAll();

        Task<IEnumerable<TType>> GetAllAsync();

        IQueryable<TType> GetAllAttached();

        void Add(TType item);

        Task AddAsync(TType item);

        void AddRange(TType[] items);

        Task AddRangeAsync(TType[] items);

        bool SoftDelete(TType entity);

        Task<bool> SoftDeleteAsync(TType entity);

        bool HardDelete(TType entity);

        Task<bool> HardDeleteAsync(TType entity);

        bool Update(TType item);

        Task<bool> UpdateAsync(TType item);

        Task<TType?> FindByConditionsAsync(Expression<Func<TType, bool>> predicate);

        Task SaveChangesAsync();
    }
}
