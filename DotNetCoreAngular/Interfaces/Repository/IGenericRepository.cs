using DotNetCoreAngular.Models.Entity;
using System.Linq.Expressions;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// Return the <!--TEntity--> object by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByID(object id);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entityToDelete);

        void DeleteRange(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);

        #region Async Methods
        Task<TEntity> GetByIdAsync(object id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        #endregion
    }
}
