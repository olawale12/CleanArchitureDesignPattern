using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Common.Responses
{
    [Serializable]
    public class BasicResponse<M>
    {
        public bool IsSuccessful { get; set; }
        public ResponseStatus<M> Response { get; set; }



        public BasicResponse() => IsSuccessful = false;

        public BasicResponse(bool isSuccessful) => IsSuccessful = isSuccessful;
    }
    public class ResponseStatus<M>
    {

        public required string Code { get; set; }
        public required string Description { get; set; }
        public M? Data { get; set; }

        public static T Create<T>(string Code, string Message, M? data) where T : BasicResponse<M>, new()
        {
            var response = new T
            {
               // IsSuccessful = false,
                Response = new ResponseStatus<M>
                {
                    Code = Code,
                    Description = Message,
                    Data = data
                },
            };
            return response;
        }
    }
}

