namespace PetGroomingApp.Services.Core.Services
{
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;

    public abstract class BaseService<TType> : IService<TType>
    where TType : class
    {
        protected readonly IRepository<TType, Guid> repository;

        protected BaseService(IRepository<TType, Guid> repository)
        {
            this.repository = repository;
        }
        public virtual async Task<bool> SoftDeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
            {
                return false;
            }

            var entity = await repository.GetByIdAsync(guidId);

            if (entity == null)
            {
                return false;
            }

            return await repository.SoftDeleteAsync(entity);
        }

        public virtual async Task<bool> HardDeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
            {
                return false;
            }

            var entity = await repository.GetByIdAsync(guidId);

            if (entity == null)
            {
                return false;
            }

            return await repository.HardDeleteAsync(entity);
        }

        public virtual async Task<bool> ExistsAsync(string? id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
            {
                return false;
            }

            var entity = await repository.GetByIdAsync(guidId);
            if (entity == null) return false;

            var isDeletedProp = typeof(TType).GetProperty("IsDeleted");
            if (isDeletedProp != null)
            {
                var isDeleted = (bool?)isDeletedProp.GetValue(entity);
                return isDeleted != true;
            }

            return true;
        }
    }

}
