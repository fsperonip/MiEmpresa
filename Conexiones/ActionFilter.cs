using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class MiFiltroPreActionyPostAction : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        var key = actionContext.HttpContext.Request.Query["key"];
        if (key != Conexiones.Conexiones.apiKey)
            actionContext.Result = new UnauthorizedObjectResult("User is unauthorized");

    }

    public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
    {
        //....
    }

}
