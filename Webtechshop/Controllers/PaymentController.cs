using Microsoft.AspNetCore.Mvc;
using Webtechshop.Services.Momo;
using Webtechshop.Models;



namespace Webtechshop.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfomation model)
        {
            var response = await _momoService.CreatePaymentMomo(model);
            
            return Redirect(response.PayUrl);
        }
        
    }
}
