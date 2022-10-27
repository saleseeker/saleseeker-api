using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using saleseeker_data;

namespace saleseeker_api.ViewModels.Subscribe
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Edit
    {
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public int ItemId { get; set; }
        public decimal NotificationThreshold { get; set; }
        public bool IsEnabled { get; set; }

        public Edit()
        {

        }
        
        public string EditSubscription(SSDbContext _db, Edit editSubscription)
        {
            //Get all the users items subscribed 
            var usersSubs = _db.SubscribedItems.Where(si => si.ItemId == editSubscription.ItemId
                                                         && si.UserId == editSubscription.UserId
                                                         && si.SiteId == editSubscription.SiteId)
                                                .FirstOrDefault();

            if (usersSubs == null)
            {
                return "not found";
            }
            usersSubs.NotificationThreshold = editSubscription.NotificationThreshold;
            //TODO: Add IsEnabled update column
            //usersSubs.IsEnabled = editSubscription.IsEnabled;

            _db.SubscribedItems.Update(usersSubs);
            _db.SaveChanges();

            return "item subscription updated";
        }
    }
}
