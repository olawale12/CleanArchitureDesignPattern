using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.MessageProviders;
using SmartCleanArchitecture.Application.Common.Responses;
using System.Net;

namespace SmartCleanArchitecture.Api.Filters
{
    public class CountryFilter : IAsyncActionFilter
    {
        private const string HEADER_KEY = "CountryCode";
        private readonly IMessageProvider _messageProvider;
        private readonly ICountryService _countryService;

        public CountryFilter(IMessageProvider messageProvider, ICountryService countryService)
        {
            _countryService = countryService;
            _messageProvider = messageProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string countryCode;
            if (!context.HttpContext.Request.Headers.TryGetValue(HEADER_KEY, out var headers))
            {
                countryCode = "01";

            }
            else
            {
                countryCode = headers.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(countryCode))
            {
                context.Result = new ObjectResult(ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_COUNTRY_CODE, _messageProvider.GetMessage(ResponseCodes.INVALID_COUNTRY_CODE), null))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            try
            {

                _countryService.SetCountryCode(countryCode);
                await next();


            }
            catch (Exception e)
            {
                Log.Information($"LanguageFilter System error {JsonConvert.SerializeObject(e)}");
                context.Result = new ObjectResult(ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_COUNTRY_CODE, _messageProvider.GetMessage(ResponseCodes.INVALID_COUNTRY_CODE), null))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
