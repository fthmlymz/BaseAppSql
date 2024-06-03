using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Common.Filters
{
    /// <summary>
    /// A filter that validates the JSON model state before executing the action.
    /// </summary>
    public class ValidateJsonModelFilter : IActionFilter
    {
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext"/> for the action that has executed.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// Called before the action executes, after the action arguments are bound.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/> for the action that is going to be executed.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
