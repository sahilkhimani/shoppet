using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IGenericService<T, TAdd, TUpdate> where T : class where TAdd : class where TUpdate : class 
    {
        Task<APIResponse<T>> Add(TAdd dto);
        Task<APIResponse<T>> Update(object id,TUpdate dto);
        Task<APIResponse<T>> Delete(object id);
        Task<APIResponse<T>> GetById(object id);
        Task<APIResponse<IEnumerable<T>>> GetAll();
        public string ApplyTitleCase(string name);
    }
}
