using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace webhook.sender.Controllers
{
    // This controller handles HTTP requests at the /api/webhook route.
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        // The constructor receives an HttpClient instance via dependency injection.
        // HttpClient is used to make HTTP requests to external services.
        public WebhookController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // This action method handles HTTP POST requests to /api/webhook/send.
        // It simulates sending a webhook to another server.
        [HttpPost("send")]
        public async Task<IActionResult> SendWebhook()
        {
            try
            {
                // Create a payload representing the event data to send in the webhook.
                // This is an anonymous object containing details about a completed sale.
                var payload = new
                {
                    _event = "sale_completed", // The type of event that occurred.
                    _data = new
                    {
                        product_id = "12345",       // The ID of the product that was sold.
                        quantity_sold = 1,          // The number of units sold.
                        sale_id = "abc123",         // A unique identifier for the sale.
                        timestamp = "2024-06-04T12:34:56Z" // The time when the sale occurred.
                    }
                };

                // Serialize the payload object to a JSON string.
                // JsonSerializer is a part of the System.Text.Json namespace.
                var jsonPayload = JsonSerializer.Serialize(payload);

                // Create an HttpContent object to hold the JSON payload.
                // This object represents the body of the HTTP request.
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                // Send an HTTP POST request to the webhook receiver's endpoint.
                // The endpoint URL is hardcoded as "https://localhost:7094/api/webhooks/receive".
                var response = await _httpClient.PostAsync("https://localhost:7094/api/webhooks/receive", content);

                // Check if the response from the receiver indicates success (HTTP 2xx status codes).
                if (response.IsSuccessStatusCode)
                {
                    // If the request was successful, return an HTTP 200 OK status with a success message.
                    return Ok("Webhook sent successfully");
                }
                else
                {
                    // If the request was not successful, return an appropriate HTTP status code
                    // and a failure message.
                    return StatusCode((int)response.StatusCode, "Failed to send webhook");
                }
            }
            catch (Exception e)
            {
                // If an exception occurs during the process, return an HTTP 400 Bad Request status
                // with the exception message.
                return BadRequest(e.Message);
            }
        }
    }
}
