using dev_pay.Interfaces;
using System.Net.Http.Headers;

namespace dev_pay.Integrations
{
    public class VTUService : IVTUService
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;
        private readonly IUtility utils;

        public VTUService(HttpClient _client, IConfiguration _config, IUtility _utils)
        {
            client = _client;
            config = _config;
            utils = _utils;
            client.BaseAddress = new Uri("https://vtu.ng/wp-json/api/v1");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
