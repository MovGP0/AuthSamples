using System.Net;

namespace Shared;

public static class HttpHelper
{
    public static HttpResponseMessage Ok(string message = "")
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            RequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(message)
            }
        };
    }

    public static HttpResponseMessage Unauthorized(string message = "")
    {
        return new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            RequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(message)
            }
        };
    }
}

public static class AuthScheme
{
    public const string Cookie = "cookie";
}