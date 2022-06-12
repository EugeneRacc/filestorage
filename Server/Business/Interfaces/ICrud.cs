namespace Business.Interfaces;

public interface ICrud<TModel> where TModel : class
{
    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> GetByIdAsync(int id);

    Task AddAsync(TModel model);

    Task UpdateAsync(TModel model);

    Task DeleteAsync(int modelId);

}