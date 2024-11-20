namespace shoppetApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        Task Update(object id, T entity);
        Task Delete(object id);
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetAll();
    }
}
