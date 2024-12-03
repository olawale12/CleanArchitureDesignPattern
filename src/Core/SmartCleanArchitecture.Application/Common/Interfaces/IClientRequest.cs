using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Interfaces
{
    public interface IClientRequest
    {
        Task<T> PostMessage<T, U>(U request, string path, HttpClient client, CancellationToken cancellationToken = default);
        Task<T> GetMessage<T>(string path, HttpClient client, CancellationToken cancellationToken = default);
    }
}
