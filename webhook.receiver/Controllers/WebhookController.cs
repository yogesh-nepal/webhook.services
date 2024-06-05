using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace webhook.receiver.Controllers
{
    // This controller handles HTTP requests at the /api/webhook route.
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        // This action method handles HTTP POST requests to /api/webhook/receive.
        // It receives and processes webhook payloads.
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveWebhook()
        {
            // Read the body of the HTTP request asynchronously.
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                // Deserialize the JSON payload into a WebhookPayload object.
                var webhookData = JsonSerializer.Deserialize<WebhookPayload>(body);

                // Check if the event type is "sale_completed".
                if (webhookData?._event == "sale_completed")
                {
                    // Process the webhook event, such as updating the inventory.
                    UpdateInventory(webhookData._data.ProductId, webhookData._data.QuantitySold);

                    // Return a 200 OK status with a success message.
                    return Ok(new { status = "success" });
                }

                // If the event type is not "sale_completed", return a 200 OK status with an ignored message.
                return Ok(new { status = "ignored" });
            }
        }

        // This method updates the inventory based on the product ID and quantity sold.
        private void UpdateInventory(string productId, int quantitySold)
        {
            // Static JSON data representing the inventory (for demonstration purposes).
            var inventory = new List<Product>
            {
                new Product { Id = "12345", Name = "Product 1", Stock = 10 },
                new Product { Id = "67890", Name = "Product 2", Stock = 20 }
            };

            // Find the product in the inventory by product ID.
            var product = inventory.FirstOrDefault(p => p.Id == productId);

            // If the product is found, update its stock.
            if (product != null)
            {
                product.Stock -= quantitySold;

                // Log the updated inventory to the console (for demonstration purposes).
                Console.WriteLine($"Updated inventory for product {productId}: New stock is {product.Stock}");
            }
        }

        // Class representing the structure of the webhook payload.
        public class WebhookPayload
        {
            public string _event { get; set; } // The type of event.
            public Data _data { get; set; }    // The data associated with the event.
        }

        // Class representing the data part of the webhook payload.
        public class Data
        {
            public string ProductId { get; set; }  // The ID of the product.
            public int QuantitySold { get; set; } // The quantity sold.
        }

        // Class representing a product in the inventory.
        public class Product
        {
            public string Id { get; set; }    // The ID of the product.
            public string Name { get; set; }  // The name of the product.
            public int Stock { get; set; }    // The stock level of the product.
        }
    }
}
