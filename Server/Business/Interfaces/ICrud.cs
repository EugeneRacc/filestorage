using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    /// <summary>
    /// Interface with CRUD methods
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface ICrud<TModel> where TModel : class
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Return IEnumerable of some Entities</returns>
        Task<IEnumerable<TModel>> GetAllAsync();
        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return some Entities</returns>
        Task<TModel> GetByIdAsync(int id);
        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task</returns>
        Task AddAsync(TModel model);
        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task</returns>
        Task UpdateAsync(TModel model);
        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <returns>Task</returns>
        Task DeleteAsync(int modelId);

    }
}