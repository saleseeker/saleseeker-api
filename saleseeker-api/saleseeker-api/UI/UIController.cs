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
            var items = new List<UIItem>();
            if (_context.Items == null)
            {
                return items;
            }

            // TODO this doesn't work :P

            var queryItems = from siteItem in _context.Set<SiteItem>()
            join scrapedItem in _context.Set<ScrapedItem>()
            on siteItem.SiteItemId equals scrapedItem.SiteItemId
            into g
            select new { siteItem.ItemId, siteItem.Item.ItemName, siteItem.Item.ItemPhotoUrl, ave = g.Average( x=>x.PriceIncVat)};

            foreach (var item in queryItems)
            {
                items.Add(new UIItem(item.ItemId, item.ItemName, item.ItemPhotoUrl, item.ave, new List<UISiteItem>()));
            }

            return items;
        }
        #endregion
    }
}
