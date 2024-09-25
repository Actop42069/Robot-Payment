using Microsoft.AspNetCore.Mvc;
using PaymentApiProject.Models;
using PaymentApiProject.Services;
using PaymentApiTestProject.Models;
using System.Threading.Tasks;

namespace PaymentApiProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentApiClient _paymentApiClient;

        public PaymentController()
        {
            _paymentApiClient = new PaymentApiClient();
        }

        // Display the payment form
        public IActionResult Index()
        {
            return View();
        }

        // Handle form submission and initiate payment
        [HttpPost]
        public async Task<IActionResult> MakePayment(PaymentRequest paymentRequest)
        {
            // Execute the payment through the API client
            var response = await _paymentApiClient.ExecutePaymentAsync(
                paymentRequest.StoreId,
                paymentRequest.Amount,
                paymentRequest.Tax,
                paymentRequest.ShippingFee
            );

            // Prepare the response object to send to the result page
            var paymentResponse = new PaymentResponse
            {
                Status = "Success",
                Message = response ?? "Payment failed"
            };

            // Redirect to the result view
            return View("PaymentResult", paymentResponse);
        }
    }
}
