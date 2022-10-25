using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saleseeker_api.Webscraper.Models;
using saleseeker_data;
using System.Text.Json;
namespace saleseeker_api.Webscraper
{
    [Authorize]
    [Route("api/webscraper")]
    [ApiController]
    public class WebscraperController : ControllerBase
    {
        //TODO: 
        // 1: Add an endpoint that when hit, hits the webscraper to trigger a scrape for the items in the db
        // 2: Add a queue reader to receive messages from the scraper and insert the data to the db

        private readonly SSDbContext _context;
        private static readonly HttpClient client = new HttpClient();
        public WebscraperController(SSDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> PostScrapedItems()
        {
            var itemsToScrape = new WebscraperItem().AllItems(_context);
            if (itemsToScrape == null || itemsToScrape.Count < 1)
            {
                // No point in scraping
                Console.WriteLine("No items in db to scrape");
                return BadRequest("No items in db to scrape");
            }

            StringContent httpContent = new StringContent(
                JsonSerializer.Serialize(itemsToScrape), 
                System.Text.Encoding.UTF8, 
                "application/json");

            // Yucky, need to improve the security so we're not relying on hardcoded values here
            client.DefaultRequestHeaders.Add("password", "30107B90-93CE-4AF4-9D72-14F984144407");
            var response = await client.PostAsync("https://saleseeker-webscraper-webapp.azurewebsites.net/api/HttpTrigger", httpContent);

            Console.WriteLine("Made request to WebScraper: " + response.ToString());

            return Ok(response);
        }



    }
}
