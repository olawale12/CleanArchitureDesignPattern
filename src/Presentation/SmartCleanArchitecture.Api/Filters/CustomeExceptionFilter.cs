using Microsoft.AspNetCore.Mvc.Filters;
using SmartCleanArchitecture.Application.Common.Interfaces;

namespace SmartCleanArchitecture.Api.Filters
{
    public class CustomeExceptionFilter : IAuthorizationFilter
    {
        private static IMessageProvider _messageProvider;

        public CustomeExceptionFilter(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           // throw new NotImplementedException();
        }
    }
}
