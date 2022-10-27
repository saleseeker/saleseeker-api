using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using saleseeker_data;

namespace saleseeker_api.ViewModels.Subscribe
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Create
    {
        public string Email { get; set; }
        public int ItemId { get; set; }
        public decimal BasePrice { get; set; }
        public decimal NotificationThreshold { get; set; }
        public bool IsEnabled { get; set; }
        public List<int> Sites { get; set; }
        public Create()
        {

        }
        public string CreateSubscription(SSDbContext _db, Create subscriptions)
        {
            var subscribedUser = FindSubscribedUser(_db, subscriptions.Email);
            var newSubscription = CreateSiteSubscriptions(_db, subscribedUser.UserId, subscriptions);
            return newSubscription;
        }

        private SubscribedUser FindSubscribedUser(SSDbContext _db, string Email)
        {
            var subscribedUser = _db.SubscribedUsers
                                    .Where(su => su.EmailAddress == Email)
                                    .FirstOrDefault();

            if (subscribedUser != null)
            {
                return subscribedUser;
            }

            // if the there is no associated email we create a new subscribe user
            subscribedUser = new SubscribedUser
            {
                EmailAddress = Email,
                FirstName = "",
                SubscribedDate = DateTime.Now,
                CellNumber = "",
                Surname = ""
            };

            _db.SubscribedUsers.Add(subscribedUser);
            _db.SaveChanges();
            return subscribedUser;
        }
        private string CreateSiteSubscriptions(SSDbContext _db, int userId, Create subscriptions)
        {
            //Get all the users items subscribed 
            var usersSubs = _db.SubscribedItems.Where(si =>
                                            si.ItemId == subscriptions.ItemId
                                         && si.UserId == userId);
            var subscribedItems = "[";
            //Loop through the sites requested subs
            foreach (var siteId in subscriptions.Sites)
            {

                //check if the theres already a subscription 
                if (usersSubs.Select(us => us.SiteId).Contains(siteId))
                {
                    continue;
                }

                var newSiteSubscription = new SubscribedItem
                {
                    SiteId = siteId,
                    UserId = userId,
                    ItemId = subscriptions.ItemId,
                    BasePrice = subscriptions.BasePrice,
                    NotificationThreshold = subscriptions.NotificationThreshold,
                    //TODO update database with isEnabled for subcribered item
                    //IsEnabled = true
                };

                string.Concat(subscribedItems, "{item:"+subscriptions.ItemId+",sites:"+siteId+"},");
                _db.SubscribedItems.Add(newSiteSubscription);
            };
            _db.SaveChanges();
            
            return string.Concat(subscribedItems, "]"); ;
        }
    }
}
