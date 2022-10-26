using Azure;
using Azure.Messaging.EventGrid;
using saleseeker_api.UI.Models;
using saleseeker_data;

namespace saleseeker_api.Webscraper.Models
{
    public class WebscraperItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public float price { get; set; }
        public string selector { get; set; }
        public string priceRegex { get; set; }

        public List<WebscraperItem> AllItems(SSDbContext _context)
        {
            return _context
                .SiteItems
                .Select(item =>
                         new WebscraperItem
                         {
                             id = item.SiteItemId,
                             name = item.Item.ItemName,
                             url = item.ItemUrl,
                             selector = item.Site.CssSelector,
                             priceRegex = item.Site.PriceRegex
                         })
                .ToList() ?? new List<WebscraperItem>();
        }

        public async Task<int> UpdateItemAsync(SSDbContext _context)
        {
            var vatPrice = price * 0.85;

            var scrapedItem = new ScrapedItem() {
                SiteItemId = id,
                PriceIncVat = (decimal)price,
                PriceExVat = (decimal?)vatPrice,
                ScrapedDateTime = DateTime.Now
            };

            var result = _context.SiteItems.First(a => a.ItemId == id);
            result.ScrapedItems.Add(scrapedItem);

            var resultCount = _context.SaveChanges();

            await SendEmailAsync(_context);
            return resultCount;
        }

        private async Task<bool> SendEmailAsync(SSDbContext _context)
        {
            try
            {
                // This is definately not the best way to do this, but time is tight so it'll have to do
                decimal averagePrice = _context.SiteItems
                    .First(x => x.SiteItemId == id)
                    .ScrapedItems
                    .Average(y => y.PriceIncVat);

                if (averagePrice <= (decimal)price)
                {
                    return false;
                }

                // This isn't accounting for any discount percentages yet. Can you take a look at this Jen?
                var subscriptions = _context.SubscribedItems
                    .Where(x => 
                        (x.Item.SiteItems
                        .Where(y => y.SiteItemId == id)
                        .Count() > 0));

                var emailsToSendTo = subscriptions
                    .Select(x => x.SubscribedUser.EmailAddress)
                    .ToList();

                if (emailsToSendTo != null && emailsToSendTo.Count() > 0)
                {
                    // Need to change the object that's deserialized and sent to one that Jeff can use.
                    // Not sure of the structure right now, so I'm just focused on implementing the event-grid calling.

                    EventGridPublisherClient client = new EventGridPublisherClient(
                        new Uri("https://saleseeker-egt-notify.eastus-1.eventgrid.azure.net/api/events"),
                        new AzureKeyCredential("BJW8PSlAREiK0LjAcnARSck+0aMNqYeAAUJAXLa0ZOE="));

                    string itemJson = System.Text.Json.JsonSerializer.Serialize(emailsToSendTo);
                    Console.WriteLine("Publishing the following to the event queue: " + itemJson);

                    // Add EventGridEvents to a list to publish to the topic
                    EventGridEvent egEvent =
                        new EventGridEvent(
                            "EmailRequest",
                            "EmailRequest",
                            "1.0",
                            itemJson);

                    // Send the event
                    var result = await client.SendEventAsync(egEvent);

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error attempting to add email request to event queue: " + e.ToString());
                return false;
            }

        }
    }
}
