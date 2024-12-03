using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.Utils;
using System.Diagnostics;
using System.Text;

namespace SmartCleanArchitecture.Application.Common.HttpClientRequest
{
    public class ClientRequest : IClientRequest
    {
        public async Task<T> PostMessage<T, U>(U request, string path, HttpClient client, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch1 = Stopwatch.StartNew();

            // Log.Information($"SERVICE REQUEST {typeof(U).Name} FI Endpoint: {client.BaseAddress}{path}");

            var input = GeneralUtil.SerializeAsJson(request);
            var message = new StringContent(input, Encoding.UTF8, "application/json");
            var rawResponse = await client.PostAsync(path, message);
            var body = await rawResponse.Content.ReadAsStringAsync();
            if (!rawResponse.IsSuccessStatusCode)
            {
                // Log error or throw exception based on status code

                throw new Exception($"Error: {rawResponse.StatusCode} - {body}");
            }
            stopwatch1.Stop();
            var timeTaken = stopwatch1.ElapsedMilliseconds;

            // Log.Information($"SERVICE COUNTER LOG, PostMessage API Call : urlPath=>{path}; time elapsed=>{timeTaken}");
            return GeneralUtil.DeserializeFromJson<T>(body);
        }

        public async Task<T> GetMessage<T>(string path, HttpClient client, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            HttpResponseMessage rawResponse;

            rawResponse = await client.GetAsync(path, cancellationToken);

            var body = await rawResponse.Content.ReadAsStringAsync();

            if (!rawResponse.IsSuccessStatusCode)
            {
                // Log error or throw exception based on status code
                throw new Exception($"Error: {rawResponse.StatusCode} - {body}");
            }

            stopwatch.Stop();
            var timeTaken = stopwatch.ElapsedMilliseconds;

            return GeneralUtil.DeserializeFromJson<T>(body);
        }
    }
}
