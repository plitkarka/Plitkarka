using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Plitkarka.Domain.Filters;

public class ModelStateValidationAttribute : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) {}

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
