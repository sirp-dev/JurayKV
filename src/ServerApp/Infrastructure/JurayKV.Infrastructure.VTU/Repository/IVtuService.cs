using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.VTU.Repository
{
    public interface IVtuService
    {
        Task<BalanceResponse> Balance();
        Task<AirtimeResponse> Airtime(AirtimeRequest request);
        Task<DataResponse> DataRequest(DataRequest request);
        Task<VerifyResponse> VerifyCustomerUtility(VerifyRequest request);
        Task<CableTvResponse> SubscribeTV(CableTvRequest request);
        Task<ElectricityResponse> Electricity(ElectricityRequest request);
    }
}
