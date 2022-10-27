using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using saleseeker_data;

namespace saleseeker_api.ViewModels.Subscribe
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Delete
    {
        public int ItemId { get; set; }
        public int SiteId { get; set; }
        public int UserId { get; set; }

        public Delete()
        {

        }
        public string DeleteSubscription(SSDbContext _db, Delete deleteSubscription)
        {
            //Get all the users items subscribed 
            var usersSubs = _db.SubscribedItems.Where(si => si.ItemId == deleteSubscription.ItemId
                                                         && si.UserId == deleteSubscription.UserId
                                                         && si.SiteId == deleteSubscription.SiteId)
                                                .FirstOrDefault();

            if (usersSubs == null)
            {
                return "not found";
            }

            _db.SubscribedItems.Remove(usersSubs);
            _db.SaveChanges();
            return "item subscription removed";
        }
    }
}
