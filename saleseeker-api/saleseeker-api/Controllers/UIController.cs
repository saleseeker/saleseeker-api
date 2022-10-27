using Microsoft.AspNetCore.Mvc;
using saleseeker_api.Responses;
using saleseeker_api.ViewModels.UI;
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
                var result = new UISite().Site(_context, id);

                if (result == null)
                {
                    return new WrapperResponse(surfixSourceName, HttpStatusCode.NotFound, "unsuccessful response");
                }

                return ResponseCreation.CreateSuccessResponse(surfixSourceName, result, message: "successful response");

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
        public WrapperResponse GetAllItemsWithAvePrices()
        {
            try
            {
                var results = new UIItem().AllItems(_context);
                return ResponseCreation.CreateSuccessResponse(surfixSourceName, results, message: "successful response");
            }
            catch (Exception ex)
            {
                return ResponseCreation.CreateErrorResponse(surfixSourceName, ex.Message);
            }
        }
        #endregion
    }
}
