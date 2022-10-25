using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Azure.Messaging.EventGrid.SystemEvents;
using saleseeker_api.Webscraper.Models;
using saleseeker_data;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [ApiController]
    [Route("api/eventgrid")]
    public class EventGridController : Controller
    {
        private readonly SSDbContext _context;

        public EventGridController(SSDbContext context)
        {
            _context = context;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("This works");
        }

        // POST: api/eventgrid/hook
        [HttpPost("hook")]
        public IActionResult ReceiveEvent([FromBody] EventGridEvent[] request)
        {
            foreach (EventGridEvent eventGridEvent in request)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        // Do any additional validation (as required) and then return back the below response

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };
                        return Ok(responseData);
                    }                    
                }
                // Handle custom events
                else if (eventGridEvent.EventType == "our.own.topic")
                {
                    // TODO: do our own shit
                }
                switch (eventGridEvent.EventType)
                {
                    case "scrapeResponse":
                        ReceiveWebscraperResponseEvent(eventGridEvent);
                        break;
                    default:
                        break;
                }
                
            }
            return Ok();
        }

        public IActionResult ReceiveWebscraperResponseEvent(EventGridEvent eventGridEvent)
        {
            Console.WriteLine("Received event from WebScraper. ");

            String eventDataString = eventGridEvent.Data.ToString();

            WebscraperItem? eventData = JsonSerializer.Deserialize<WebscraperItem>(eventDataString);
            if (eventData != null && eventData.id > 0)
            {
                // Do any additional validation (as required) and then return back the below response
                eventData.UpdateItem(_context);
                Console.WriteLine("Updated item: " + eventData.id + " New price: " + eventData.price);
            }
            // Handle custom events
            else if (eventGridEvent.EventType == "our.own.topic")
            {
                // TODO: do our own shit
            }
            return Ok();
        }

        public void PostEvent(string topic, object data)
        {
            // TODO make this actually do something
        }
    }
}