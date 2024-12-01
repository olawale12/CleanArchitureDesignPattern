using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using SmartCleanArchitecture.Application.Common.Exceptions;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Responses;
using System.Net;

namespace SmartCleanArchitecture.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {

        public override void OnException(ExceptionContext context)
        {

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = context.Exception is ValidationException
                ? ProcessValidationErrors(context)
                : ProcessSystemErrors(context);

            context.Result = new JsonResult(response);
        }

        private static PayloadResponse<string> ProcessSystemErrors(ExceptionContext context)
        {
            Log.Error(context.Exception, "[Error]");
            var message = "Unable to complete your transaction";
            // _messageProvider.GetMessage(ResponseCodes.SYSTEM_ERROR)
            return ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.SYSTEM_ERROR, message, null);
        }

        private PayloadResponse<string> ProcessValidationErrors(ExceptionContext context)
        {
            var validationErrors = ((ValidationException)context.Exception).Errors;
            var message = validationErrors.FirstOrDefault().Value.FirstOrDefault();
            return ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.SYSTEM_ERROR, message, null);
        }

    }
}
