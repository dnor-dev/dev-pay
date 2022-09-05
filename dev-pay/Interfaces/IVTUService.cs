using dev_pay.Models.VTU;

namespace dev_pay.Interfaces
{
    public interface IVTUService
    {
        Task<AirtimeResponseModel> BuyAirtime(AirtimeRequestModel model);
        Task<VerifyVTUResponse> Verify(VerifyVTUServiceModel model);
        Task<CableTVResponse> SubscribeCableTV(CableTVModel model);
        Task<PayElectricityResponse> PayElectricity(PayElectricityModel model);
    }
}
