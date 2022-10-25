using System.Net;

namespace saleseeker_api.Responses
{
    public static partial class ResponseCreation
    {
        internal static WrapperResponse CreateErrorResponse(string source, string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            return new WrapperResponse(source, httpStatusCode, message);
        }

        internal static WrapperResponse CreateSuccessResponse(string source, object? result, string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return new WrapperResponse(source, httpStatusCode, message, result);
        }
    }
}
