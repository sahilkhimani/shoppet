using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace shoppetApi.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _ModelState = context.ModelState;
            if (!_ModelState.IsValid) {
                context.Result = new BadRequestObjectResult(_ModelState);
            }
        }
    }
}
