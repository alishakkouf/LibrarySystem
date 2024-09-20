using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateModelStateFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var model = context.ActionArguments.FirstOrDefault(arg => arg.Value is IValidation).Value as IValidation;

        if (model != null)
        {
            if (!model.IsValid() || !context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Message = model?.ValidationMessage ?? "Invalid data",
                    Errors = context.ModelState.SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                });
            }
        }
        else if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = "Invalid data",
                Errors = context.ModelState.SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
            });
        }

        base.OnActionExecuting(context);
    }
}
