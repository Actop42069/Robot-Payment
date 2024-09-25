using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentApiProject.Services
{
    public class PaymentApiClient
    {
        private readonly HttpClient _httpClient;

        public PaymentApiClient()
        {
            _httpClient = new HttpClient();
        }

        // Method to perform a single payment request
        public async Task<string> ExecutePaymentAsync(string storeId, decimal amount, decimal tax, decimal shippingFee, string jobType = "CAPTURE")
        {
            // Define the payment URL based on the documentation
            var paymentUrl = "https://credit.j-payment.co.jp/link/creditcard";

            // Create the request content with required parameters
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("aid", storeId),          // Store ID
                new KeyValuePair<string, string>("am", amount.ToString()),  // Amount
                new KeyValuePair<string, string>("tx", tax.ToString()),     // Tax
                new KeyValuePair<string, string>("sf", shippingFee.ToString()), // Shipping Fee
                new KeyValuePair<string, string>("jb", jobType)            // Job Type (CAPTURE or AUTH)
            });

            try
            {
                // Send POST request to the API
                var response = await _httpClient.PostAsync(paymentUrl, content);

                // Check if it was redirected to an error page
                if (response.StatusCode == System.Net.HttpStatusCode.Found)
                {
                    var redirectUrl = response.Headers.Location?.ToString();
                    Console.WriteLine($"Redirected to: {redirectUrl}");
                    return null;
                }

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read and return the response body
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors (common for redirects)
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return ex.Message;
            }
            catch (Exception ex)
            {
                // Handle other general exceptions
                Console.WriteLine($"Error occurred: {ex.Message}");
                return ex.Message;
            }
        }
    }
}
