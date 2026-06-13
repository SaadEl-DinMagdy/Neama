using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Stripe;

namespace Neama.Api.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentservice;
        // ملاحظة: يفضل نقل الـ Secret إلى ملف appsettings.json في مشاريع الإنتاج للحماية
        private string _whSecret = "whsec_f370c9ec900f4bf0bed528234c645ab2ea6201086df45c3f8b2647fc64df30ee";

        public PaymentsController(IPaymentService paymentservice)
        {
            _paymentservice = paymentservice;
        }

        [HttpPost("{basketid}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntint(string basketId)
        {
            var basket = await _paymentservice.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) { return BadRequest(new ApiResponse(400)); }

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    await _paymentservice.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                    break;
                case "payment_intent.payment_failed":
                    await _paymentservice.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    break;
            }
            return Ok();
        }
    }
}
