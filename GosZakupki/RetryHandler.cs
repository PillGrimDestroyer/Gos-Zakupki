using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GosZakupki
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MAX_RETRIES = 3;

        public RetryHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = default(HttpResponseMessage);

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return response;
        }
    }
}
