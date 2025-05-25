using Webtechshop.Models;
using Webtechshop.Models.Momo;

namespace Webtechshop.Services.Momo
{
    public interface IMomoService 
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfomation model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
