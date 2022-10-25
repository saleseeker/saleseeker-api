using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Azure.Messaging.EventGrid.SystemEvents;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [ApiController]
    [Route("api/eventgrid")]
    public class EventGridController : Controller
    {
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
            }
            return BadRequest();
        }

        public void PostEvent(string topic, object data)
        {
            // TODO make this actually do something
        }
    }
}