using Application.Common.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Shared;

namespace Web.Api.Test.Core.Application.Common.Filters
{
    [TestFixture]
    public class ValidateFilterAttributeTests
    {
        [Test]
        public void OnActionExecuting_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var attribute = new ValidateFilterAttribute();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Key", "Error message");
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                modelState
            );
            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new MockController()
            );

            // Act
            attribute.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionExecutingContext.Result);
            var badRequestResult = (BadRequestObjectResult)actionExecutingContext.Result;
            Assert.IsInstanceOf<Result<NoContent>>(badRequestResult.Value);
            var result = (Result<NoContent>)badRequestResult.Value;
            Assert.IsFalse(result.Succeeded);
          //  Assert.AreEqual("Error message", result.Exception.Message);
        }
    }
    public class MockController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
