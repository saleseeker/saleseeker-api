using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using saleseeker_data;

namespace saleseeker_api.Controllers
{
    [Authorize]
    [Route("api/subscribeditems")]
    [ApiController]
    public class SubscribedItemsController : ControllerBase
    {
        private readonly SSDbContext _context;

        public SubscribedItemsController(SSDbContext context)
        {
            _context = context;
        }

        #region CRUD

        // GET: api/SubscribedItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscribedItem>>> GetSubscribedItems()
        {
          if (_context.SubscribedItems == null)
          {
              return NotFound();
          }
            return await _context.SubscribedItems.ToListAsync();
        }

        // GET: api/SubscribedItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscribedItem>> GetSubscribedItem(int id)
        {
          if (_context.SubscribedItems == null)
          {
              return NotFound();
          }
            var subscribedItem = await _context.SubscribedItems.FindAsync(id);

            if (subscribedItem == null)
            {
                return NotFound();
            }

            return subscribedItem;
        }

        // PUT: api/SubscribedItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscribedItem(int id, SubscribedItem subscribedItem)
        {
            if (id != subscribedItem.SubscribedItemId)
            {
                return BadRequest();
            }

            _context.Entry(subscribedItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscribedItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubscribedItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscribedItem>> PostSubscribedItem(SubscribedItem subscribedItem)
        {
          if (_context.SubscribedItems == null)
          {
              return Problem("Entity set 'SSDbContext.SubscribedItems'  is null.");
          }
            _context.SubscribedItems.Add(subscribedItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscribedItem", new { id = subscribedItem.SubscribedItemId }, subscribedItem);
        }

        // DELETE: api/SubscribedItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscribedItem(int id)
        {
            if (_context.SubscribedItems == null)
            {
                return NotFound();
            }
            var subscribedItem = await _context.SubscribedItems.FindAsync(id);
            if (subscribedItem == null)
            {
                return NotFound();
            }

            _context.SubscribedItems.Remove(subscribedItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscribedItemExists(int id)
        {
            return (_context.SubscribedItems?.Any(e => e.SubscribedItemId == id)).GetValueOrDefault();
        }

        #endregion

        #region other



        #endregion
    }
}
