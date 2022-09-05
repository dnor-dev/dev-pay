using dev_pay.Interfaces;
using dev_pay.Models.VTU;
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

        public async Task<AirtimeResponseModel> BuyAirtime(AirtimeRequestModel model)
        {
            var res = await client.GetAsync($"airtime?username={config["VTUUsername"]}&password={config["VTUPassword"]}&phone={model.phone}&network_id={model.network_id}&amount={model.amount / 100}");

            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<AirtimeResponseModel>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<AirtimeResponseModel>();
            return obj;
        }

        public async Task<VerifyVTUResponse> Verify(VerifyVTUServiceModel model)
        {
            var res = await client.GetAsync($"verify-customer?username={config["VTUUsername"]}&password={config["VTUPassword"]}&customer_id={model.customer_id}&service_id={model.service_id}&variation_id={model.variation_id}");

            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<AirtimeResponseModel>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<VerifyVTUResponse>();
            return obj;
        }

        public async Task<CableTVResponse> SubscribeCableTV(CableTVModel model)
        {
            var res = await client.GetAsync($"tv?username={config["VTUUsername"]}&password={config["VTUPassword"]}&phone={model.phone}&service_id={model.service_id}&smartcard_number={model.smartcard_number}&variation_id={model.variation_id}");

            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<CableTVResponse>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<CableTVResponse>();
            return obj;
        }

        public async Task<PayElectricityResponse> PayElectricity(PayElectricityModel model)
        {
            var res = await client.GetAsync($"electricity?username={config["VTUUsername"]}&password={config["VTUPassword"]}&phone={model.phone}&meter_number={model.meter_number}&service_id={model.service_id}&variation_id={model.variation_id}&amount={model.amount}");

            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<PayElectricityResponse>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<PayElectricityResponse>();
            return obj;
        }
    }
}
