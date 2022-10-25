using Microsoft.AspNetCore.Mvc;

namespace saleseeker_api.Webscraper
{
    public class WebscraperController : ControllerBase
    {
        //TODO: 
        // 1: Add an endpoint that when hit, hits the webscraper to trigger a scrape for the items in the db
        // 2: Add a queue reader to receive messages from the scraper and insert the data to the db
    }
}
