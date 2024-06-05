# .NET 8 Webhook Example

This repository contains two .NET 8 projects demonstrating how to implement webhook functionality for sending and receiving webhook payloads.

## Webhook Sender

The Webhook Sender project demonstrates how to send webhook payloads using .NET 8. It includes a controller (WebhookController) with an action method (SendWebhook) that sends a sample webhook payload to a receiver endpoint. This project serves as an example of how to integrate webhook functionality into your .NET 8 applications.

### Usage

To use the Webhook Sender project:
1. Clone this repository.
2. Open the solution in Visual Studio.
3. Run the Webhook Sender project.
4. Send webhook payloads using the provided endpoint (/api/webhook/send).

## Webhook Receiver

The Webhook Receiver project showcases how to receive and process webhook payloads using .NET 8. It includes a controller (WebhookController) with an action method (ReceiveWebhook) that processes incoming webhook payloads. This project demonstrates how to deserialize the JSON payload, handle the event type, and perform actions based on the webhook data.

### Usage

To use the Webhook Receiver project:
1. Clone this repository.
2. Open the solution in Visual Studio.
3. Run the Webhook Receiver project.
4. Configure the sender to send webhook payloads to the receiver endpoint (/api/webhook/receive).
