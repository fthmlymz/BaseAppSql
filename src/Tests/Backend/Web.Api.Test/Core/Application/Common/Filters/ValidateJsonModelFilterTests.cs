using Application.Common.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Api.Test.Core.Application.Common.Filters
{
    [TestFixture]
    public class ValidateJsonModelFilterTests
    {
        private ValidateJsonModelFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new ValidateJsonModelFilter();
        }

        [Test]
        public void OnActionExecuting_WithInvalidModelState_SetsBadRequestResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new(), new(), new());
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), null);
            context.ModelState.AddModelError("PropertyName", "Invalid value");

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
            var badRequestResult = (BadRequestObjectResult)context.Result;
            var serializableError = (SerializableError)badRequestResult.Value;
            Assert.That(serializableError, Is.EqualTo(new SerializableError { { "PropertyName", new string[] { "Invalid value" } } }));
        }

        [Test]
        public void OnActionExecuting_WithValidModelState_DoesNotSetResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new(), new(), new());
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), null);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsNull(context.Result);
        }
    }
}
