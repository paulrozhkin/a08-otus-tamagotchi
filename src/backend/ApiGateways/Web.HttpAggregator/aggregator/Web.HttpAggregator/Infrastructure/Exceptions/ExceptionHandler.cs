using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.HttpAggregator.Infrastructure.Exceptions
{
    public static class ExceptionHandler
    {
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(context => WriteJsonErrorResponse(context, true));
            });
        }

        private static async Task WriteJsonErrorResponse(HttpContext httpContext, bool includeDetails)
        {
            // Try and retrieve the error from the ExceptionHandler middleware
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            // Should always exist, but best to be safe!
            if (ex != null)
            {
                // ProblemDetails has it's own content type
                httpContext.Response.ContentType = "application/problem+json";

                // Get the details to display, depending on whether we want to expose the raw exception
                var title = includeDetails ? "An error occurred: " + ex.Message : "An error occurred";
                var details = includeDetails ? ex.ToString() : null;

                var problem = new ProblemDetails
                {
                    Status = (int) HttpStatusCode.InternalServerError,
                    Title = title,
                    Detail = details
                };

                // This is often very handy information for tracing the specific request
                var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
                if (traceId != null)
                {
                    problem.Extensions["traceId"] = traceId;
                }

                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }
        }
    }
}