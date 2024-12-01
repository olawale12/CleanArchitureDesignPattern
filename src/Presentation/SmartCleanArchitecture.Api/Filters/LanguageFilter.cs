using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Responses;
using System.Net;

namespace SmartCleanArchitecture.Api.Filters
{
    public class LanguageFilter : IAsyncActionFilter
    {
        private const string HEADER_KEY = "LanguageCode";
        private readonly IMessageProvider _messageProvider;
        private readonly ILanguageService _languageService;
        public LanguageFilter(IMessageProvider messageProvider, ILanguageService languageService)
        {
            _messageProvider = messageProvider;
            _languageService = languageService;

        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string languageCode;
            if (!context.HttpContext.Request.Headers.TryGetValue(HEADER_KEY, out var headers))
            {
                context.Result = new ObjectResult(ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_LANGUAGE_CODE, _messageProvider.GetMessage(ResponseCodes.INVALID_LANGUAGE_CODE), null))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;

            }

            languageCode = headers.FirstOrDefault();


            if (string.IsNullOrEmpty(languageCode))
            {
                context.Result = new ObjectResult(ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_LANGUAGE_CODE, _messageProvider.GetMessage(ResponseCodes.INVALID_LANGUAGE_CODE), null))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            try
            {

                _languageService.SetLanguageCode(languageCode.ToLower());
                // context.ActionArguments["languageCode"] = languageCode;
                await next();


            }
            catch (Exception e)
            {
                Log.Information($"LanguageFilter System error {JsonConvert.SerializeObject(e)}");
                context.Result = new ObjectResult(ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_LANGUAGE_CODE, _messageProvider.GetMessage(ResponseCodes.INVALID_LANGUAGE_CODE), null))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

    }
}
