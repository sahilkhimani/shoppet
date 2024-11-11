using Microsoft.AspNetCore.Mvc;

namespace shoppetApi.Controllers
{
    public interface IGenericController<T> where T : class
    {
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<T>> Add([FromBody] T entity);
        Task<ActionResult<T>> Update(int id, [FromBody] T entity);
        Task<ActionResult<T>> Delete(int id);

    }
}
