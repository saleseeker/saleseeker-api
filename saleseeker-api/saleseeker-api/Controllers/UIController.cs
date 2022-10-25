using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using saleseeker_api.Responses;
using saleseeker_api.UI.Models;
using saleseeker_data;
using System.Net;

namespace saleseeker_api.Controllers
{
    [Route("api/ui")]
    [ApiController]
    public class UIController : ControllerBase
    {
        private readonly SSDbContext _context;
        public static readonly string surfixSourceName = "ui-bff";

        public UIController(SSDbContext context)
        {
            _context = context;
        }

        #region Sites

        // GET: api/ui/sites
        [HttpGet("sites")]
        public WrapperResponse GetSites()
        {
            try
            {
                using (_context)
                {
                    var results = new UISite().Sites(_context);

                    return ResponseCreation.CreateSuccessResponse(surfixSourceName, results, message: "successful response");
                }
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }

        // GET: api/ui/sites/5
        [HttpGet("sites/{id}")]
        public WrapperResponse GetSite(int id)
        {
            try
            {
                using (_context)
                {
                    var result = new UISite().Site(_context, id);
                    
                    if (result == null)
                    {
                        return new WrapperResponse(surfixSourceName, HttpStatusCode.NotFound, "unsuccessful response");
                    }

                    return ResponseCreation.CreateSuccessResponse(surfixSourceName, result, message: "successful response");
                }
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }

        #endregion

        #region Items

        // GET: api/ui/items
        [HttpGet("items")]
        public WrapperResponse GetAllItems()
        {
            try
            {
                using (_context)
                {
                    var results = new UIItem().AllItems(_context);
                    return ResponseCreation.CreateSuccessResponse(surfixSourceName, results, message: "successful response");
                }
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }

        // GET: api/ui/itemsaveprices
        [HttpGet("itemsaveprices")]
        public WrapperResponse GetAllItemsWithAvePrices()
        {
            try
            {
                using (_context)
                {
                    // working on this - JM
                    //var q3 = (from item in _context.Set<Item>()
                    //          join siteItem in _context.Set<SiteItem>() on item.ItemId equals siteItem.ItemId
                    //          join scraped in _context.Set<ScrapedItem>() on siteItem.SiteItemId equals scraped.SiteItemId
                    //          group new { item, scraped } by new { item.ItemId, item.ItemName, item.ItemPhotoUrl } into g2
                    //          select new {
                    //              g2.Key.ItemId,
                    //              g2.Key.ItemName,
                    //              g2.Key.ItemPhotoUrl,
                    //              Ave = g2.Average(x => x.scraped.PriceIncVat),
                    //              scraped = g2.Select(a=>a.scraped)

                    //          }).ToList();

                    //foreach (var item in q3)
                    //{
                    //    var uiItem = new UIItem(item.ItemId, item.ItemName, item.ItemPhotoUrl, item.Ave, new List<UISiteItem>());
                    //    foreach (var scraped in item.scraped)
                    //    {
                    //        var uiSiteItem = new UISiteItem(item.ItemId, scraped.SiteItemId, scraped.)
                    //    }
                    //}
                    
                    var results = new UIItem().ItemsWithAvePrices(_context);
                    return ResponseCreation.CreateSuccessResponse(surfixSourceName, results, message: "successful response");
                }
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }
        #endregion
    }
}
