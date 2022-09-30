using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using saleseeker_api.UI.Models;
using saleseeker_data;

namespace saleseeker_api.UI
{
    [Route("api/ui")]
    [ApiController]
    public class UIController : ControllerBase
    {
        private readonly SSDbContext _context;

        public UIController(SSDbContext context)
        {
            _context = context;
        }

        #region Sites

        // GET: api/ui/sites
        [HttpGet("sites")]
        public IEnumerable<UISite> GetSites()
        {
            if (_context.Sites == null)
            {
                yield break;
            }
            foreach (var site in _context.Sites)
            {
                yield return new UISite(site.SiteId, site.SiteName, site.SiteLogoUrl, site.SiteHomeUrl);
            }
        }

        // GET: api/ui/sites/5
        [HttpGet("sites/{id}")]
        public ActionResult<UISite> GetSite(int id)
        {
            if (_context.Sites == null)
            {
                return NotFound();
            }

            var site = _context.Sites.FirstOrDefault(a => a.SiteId == id);
            return site == null ? NotFound() : Ok(new UISite(site.SiteId, site.SiteName, site.SiteLogoUrl, site.SiteHomeUrl));
        }

        #endregion

        #region Items

        // GET: api/ui/items
        [HttpGet("items")]
        public IEnumerable<UIItem> GetAllItems()
        {
            if (_context.Items == null)
            {
                yield break;
            }
            foreach (var item in _context.Items)
            {
                yield return new UIItem(item.ItemId, item.ItemName, item.ItemPhotoUrl);
            }
        }

        // GET: api/ui/itemsaveprices
        [HttpGet("itemsaveprices")]
        public IEnumerable<UIItem> GetAllItemsWithAvePrices()
        {
            if (_context.Items == null)
            {
                return new List<UIItem>(); ;
            }

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


            var q2 = (from item in _context.Set<Item>()
                     join siteItem in _context.Set<SiteItem>() on item.ItemId equals siteItem.ItemId
                     join scraped in _context.Set<ScrapedItem>() on siteItem.SiteItemId equals scraped.SiteItemId
                     group new { item, scraped } by new { item.ItemId, item.ItemName, item.ItemPhotoUrl } into g2
                     select new UIItem(
                         g2.Key.ItemId, 
                         g2.Key.ItemName, 
                         g2.Key.ItemPhotoUrl, 
                         g2.Average(x => x.scraped.PriceIncVat), 
                         new List<UISiteItem>()
                         )
                     ).ToList<UIItem>();


            return q2;
        }
        #endregion
    }
}
