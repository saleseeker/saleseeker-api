using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saleseeker_api.Responses;
using saleseeker_api.ViewModels.Subscribe;
using saleseeker_data;
using Index = saleseeker_api.ViewModels.Subscribe.Index;

namespace saleseeker_api.Controllers
{
    [Authorize]
    [Route("api/subscribe")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly SSDbContext _db;
        public static readonly string surfixSourceName = "subscribe-bff";

        public SubscribeController(SSDbContext db)
        {
            _db = db;
        }

        // GET: api/subscribe/index
        [HttpGet("index")]
        public WrapperResponse Index()
        {
            try
            {
                var results = new Index().AllUsersSubscribtions(_db);
                return ResponseCreation.CreateSuccessResponse(surfixSourceName, results, message: "successful response");
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }
        
        // Post: api/subscribe/create
        [HttpPost("create")]
        public WrapperResponse Create(Create subscription)
        {
            try
            {
                var result = new Create().CreateSubscription(_db,subscription);
                if (result == "[]")
                {
                    return ResponseCreation.CreateSuccessResponse(surfixSourceName, result, message: "items already subscribed"); ;
                }
                return ResponseCreation.CreateSuccessfulCreatedResponse(surfixSourceName, result, message: "items successfully subscribed");
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }

        // Put: api/subscribe/edit
        [HttpPut("edit")]
        public WrapperResponse Edit(Edit subscription)
        {
            try
            {
                var result = new Edit().EditSubscription(_db, subscription);
                
                if (result == "not found")
                {
                    return ResponseCreation.CreateNotFoundErrorResponse(surfixSourceName, result);
                }

                return ResponseCreation.CreateSuccessResponse(surfixSourceName, result, message: "items successfully subscribed");
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }

        // Delete: api/subscribe/delete
        [HttpDelete("delete")]
        public WrapperResponse Delete(Delete deleteSubscription)
        {
            try
            {
                var result = new Delete().DeleteSubscription(_db, deleteSubscription);
                
                if (result == "not found")
                {
                    return ResponseCreation.CreateNotFoundErrorResponse(surfixSourceName, result);
                }
                
                return ResponseCreation.CreateSuccessResponse(surfixSourceName, result, message: "successfully deleted subscription");
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }
    }
}
