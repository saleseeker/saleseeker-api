using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using saleseeker_data;

namespace saleseeker_api.ViewModels.Subscribe
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Index
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Subscription>? Subscriptions { get; set; }


        public List<Index> AllUsersSubscribtions(SSDbContext _db)
        {
            return _db.SubscribedUsers
                        .Include(s => s.SubscribedItems)
                        .ThenInclude(i => i.Item)
                        .ThenInclude(s => s.SiteItems)
                        .Select(subs => new Index
                        {
                            UserName = subs.FirstName,
                            Email = subs.EmailAddress,
                            Subscriptions = subs.SubscribedItems
                                    .Select(si => new Subscription
                                    {
                                        ItemId = si.ItemId,
                                        ItemName = si.Item.ItemName,
                                        SiteId = si.Site.SiteId,
                                        SiteName = si.Site.SiteName,
                                        BasePrice = si.BasePrice,
                                        PercentDiscount = si.PercentDiscount,
                                        NotificationThreshold = si.NotificationThreshold,
                                        IsEnabled = true
                                    }).ToList()
                        })
                        .ToList();
        }
    }
}
