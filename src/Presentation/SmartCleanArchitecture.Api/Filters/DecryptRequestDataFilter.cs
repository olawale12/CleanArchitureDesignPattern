using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Responses;
using SmartCleanArchitecture.Application.Common.Utils;
using SmartCleanArchitecture.Application.DTOs;
using SmartCleanArchitecture.Application.Models;
using System.Net;

namespace SmartCleanArchitecture.Api.Filters
{
    public class DecryptRequestDataFilter<T> : IAsyncActionFilter
    {
        public T RequestDataType { get; set; }
        private const string REQUEST = "EncryptedRequestData";
        private readonly IMessageProvider _messageProvider;
        private readonly ILogger<DecryptRequestDataFilter<T>> _logger;
        private readonly SystemSetting _settings;
        public DecryptRequestDataFilter(IMessageProvider messageProvider, ILogger<DecryptRequestDataFilter<T>> logger,
                IOptions<SystemSetting> settings)
        {
            _messageProvider = messageProvider;
            _logger = logger;
            _settings = settings.Value;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            object output;
            BaseEncryptedRequestDTO request;
            if (_settings.UseEncryptedData)
            {
                if (!context.ActionArguments.TryGetValue(REQUEST, out output))
                {
                    context.Result = new ObjectResult(
                        ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_INPUT_PARAMETER, _messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER), null)
                        )
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    return;
                }
                request = output as BaseEncryptedRequestDTO;
                _logger.LogInformation($"{typeof(T).Name} ENCRYPTED REQUEST ==> {GeneralUtil.SerializeAsJson(request)}");
                if (!request.IsValid(out string problemSource))
                {
                    context.Result = new ObjectResult(
                       ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_INPUT_PARAMETER, _messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER), null)
                       )
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    return;
                }

                if (!GeneralUtil.IsBase64String(request.EncryptedData))
                {
                    context.Result = new ObjectResult(
                        ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_INPUT_PARAMETER, _messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER), null)
                        )
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    return;
                }

                try
                {
                    var deserializedData = GeneralUtil.DecryptRequest<T>(request.EncryptedData, _settings.EncryptionKey);
                    if (deserializedData != null)
                    {
                        context.ActionArguments["command"] = deserializedData;
                    }
                    await next();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Decryption error");
                    var error = ResponseStatus<string>.Create<PayloadResponse<string>>(ResponseCodes.INVALID_INPUT_PARAMETER, _messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER), null);
                    context.Result = new ObjectResult(error)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
            }
            else
            {
                await next();
            }
        }
    }
}
