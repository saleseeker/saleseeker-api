using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using saleseeker_data;

namespace saleseeker_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapedItemsController : ControllerBase
    {
        private readonly SSDbContext _context;

        public ScrapedItemsController(SSDbContext context)
        {
            _context = context;
        }

        // GET: api/ScrapedItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScrapedItem>>> GetScrapedItems()
        {
          if (_context.ScrapedItems == null)
          {
              return NotFound();
          }
            return await _context.ScrapedItems.ToListAsync();
        }

        // GET: api/ScrapedItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScrapedItem>> GetScrapedItem(int id)
        {
          if (_context.ScrapedItems == null)
          {
              return NotFound();
          }
            var scrapedItem = await _context.ScrapedItems.FindAsync(id);

            if (scrapedItem == null)
            {
                return NotFound();
            }

            return scrapedItem;
        }

        // PUT: api/ScrapedItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScrapedItem(int id, ScrapedItem scrapedItem)
        {
            if (id != scrapedItem.ScrapedItemId)
            {
                return BadRequest();
            }

            _context.Entry(scrapedItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScrapedItemExists(id))
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

        // POST: api/ScrapedItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ScrapedItem>> PostScrapedItem(ScrapedItem scrapedItem)
        {
          if (_context.ScrapedItems == null)
          {
              return Problem("Entity set 'SSDbContext.ScrapedItems'  is null.");
          }
            _context.ScrapedItems.Add(scrapedItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScrapedItem", new { id = scrapedItem.ScrapedItemId }, scrapedItem);
        }

        // DELETE: api/ScrapedItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScrapedItem(int id)
        {
            if (_context.ScrapedItems == null)
            {
                return NotFound();
            }
            var scrapedItem = await _context.ScrapedItems.FindAsync(id);
            if (scrapedItem == null)
            {
                return NotFound();
            }

            _context.ScrapedItems.Remove(scrapedItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScrapedItemExists(int id)
        {
            return (_context.ScrapedItems?.Any(e => e.ScrapedItemId == id)).GetValueOrDefault();
        }
    }
}
