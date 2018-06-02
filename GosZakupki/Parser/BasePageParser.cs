using GosZakupki.Support;
using System.Net.Http;

namespace GosZakupki.Parser
{
    public class BasePageParser
    {
        protected string link;

        public BasePageParser(string link)
        {
            this.link = link;
        }

        protected HttpClient getNewHttpClient()
        {
            HttpClient client;
            HttpClientHandler handler;

            handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            client = new HttpClient(new RetryHandler(handler));

            return client;
        }
    }
}
