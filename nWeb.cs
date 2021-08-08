#pragma warning disable
using System.Net;

namespace nWeb
{
    public class newWebClient : WebClient
    {
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            (request as HttpWebRequest).AllowAutoRedirect = true;
            (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            WebResponse response = base.GetWebResponse(request);
            return response;
        }
    }
}