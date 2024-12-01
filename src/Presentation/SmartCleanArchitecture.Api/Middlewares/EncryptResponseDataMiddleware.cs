using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System.Text;
using System.Diagnostics;
using SmartCleanArchitecture.Application.Models;
using SmartCleanArchitecture.Application.Common.Utils;
using Newtonsoft.Json.Linq;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.MessageProviders;


namespace SmartCleanArchitecture.Api.Middlewares
{
    public class EncryptResponseDataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SystemSetting _settings;
        private readonly ILogger _logger;
        private readonly IMessageProvider _messageProvider;


        public EncryptResponseDataMiddleware(RequestDelegate next, IOptions<SystemSetting> settings, ILogger<EncryptResponseDataMiddleware> logger, IMessageProvider messageProvider)
        {
            _next = next;
            _logger = logger;
            _settings = settings.Value;
            _messageProvider = messageProvider;

        }

        public async Task Invoke(HttpContext context)
        {
            if (!_settings.EncryptResponseData)
            {
                await _next(context);
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Response body interceptor

            var originalBodyStream = context.Response.Body;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();
                    var currPos = memoryStream.Position;

                    // here we are tracking system error so that we can interpret the message to selectd language
                    var data = (JObject)JsonConvert.DeserializeObject(responseBodyText);
                    String code = data["response"].Value<string>("code");

                    if (code == "E1000")
                    {
                        var msg = _messageProvider.GetMessage(code);
                        data["response"]["description"] = msg;
                        responseBodyText = data.ToString();
                    }

                    var encryptedData = GeneralUtil.Encryptor(responseBodyText, _settings.EncryptionKey);
                    var jsonData = JsonConvert.SerializeObject(new { responseData = encryptedData });

                    context.Response.ContentType = "application/json; charset=UTF-8";

                    // Use StreamWriter to write the encrypted data to the response stream
                    await using (var writer = new StreamWriter(originalBodyStream, new UTF8Encoding(false), leaveOpen: true))
                    {
                        await writer.WriteAsync(jsonData);
                        await writer.FlushAsync();
                    }

                    memoryStream.Position = currPos;

                    if (_settings.AddBufferSize)
                    {
                        await memoryStream.CopyToAsync(originalBodyStream, _settings.EncryptResponseDataBufferSize);
                    }
                    else
                    {
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
            originalBodyStream.Dispose();

            #endregion

            stopwatch.Stop();
            double elapsedTime = stopwatch.ElapsedMilliseconds;
            _logger.LogInformation($"EncryptResponseData Elapsed Time: {elapsedTime} ms");
        }
    }
}
