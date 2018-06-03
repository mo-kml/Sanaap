using Newtonsoft.Json;
using Simple.OData.Client;

namespace System
{
    public class ResponseError
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ResponseErrorRoot
    {
        public ResponseError error { get; set; }
    }

    public static class ExceptionExtensions
    {
        public static string GetMessage(this Exception exp)
        {
            if (exp is WebRequestException webReq && webReq.Message == "KnownError")
            {
                return JsonConvert.DeserializeObject<ResponseErrorRoot>(webReq.Response).error.message;
            }

            return exp.Message;
        }
    }
}
