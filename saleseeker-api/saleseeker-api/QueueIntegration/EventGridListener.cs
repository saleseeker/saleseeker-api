using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Azure.Messaging.EventGrid.SystemEvents;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [Produces("application/json")]
    public class EventGridListener : Controller
    {
        [HttpPost]
        [Route("api/eventgrid")]
        public IActionResult Post([FromBody] HttpRequest request)
        {
            string response = string.Empty;
            BinaryData events = BinaryData.FromStream(request.Body);
            
            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
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
                        return new OkObjectResult(responseData);
                    }                    
                }
                // Handle custom events
                else if (eventGridEvent.EventType == "our.own.topic")
                {
                    // TODO: do our own shit
                }
            }
            return new OkObjectResult(response);
        }
    }
}