using Microsoft.AspNetCore.Mvc;

namespace shoppetApi.Controllers
{
    public interface IGenericController<T, TAdd, TUpdate> where T : class where TAdd : class where TUpdate : class
    {
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<ActionResult<T>> GetById(string id);
        Task<ActionResult<T>> Add([FromBody] TAdd dto);
        Task<ActionResult<T>> Update(string id, [FromBody] TUpdate dto);
        Task<ActionResult<T>> Delete(string id);

    }
}
