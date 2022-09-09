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
    public class NotificationSentsController : ControllerBase
    {
        private readonly SSDbContext _context;

        public NotificationSentsController(SSDbContext context)
        {
            _context = context;
        }

        // GET: api/NotificationSents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationSent>>> GetNotificationSents()
        {
          if (_context.NotificationSents == null)
          {
              return NotFound();
          }
            return await _context.NotificationSents.ToListAsync();
        }

        // GET: api/NotificationSents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationSent>> GetNotificationSent(int id)
        {
          if (_context.NotificationSents == null)
          {
              return NotFound();
          }
            var notificationSent = await _context.NotificationSents.FindAsync(id);

            if (notificationSent == null)
            {
                return NotFound();
            }

            return notificationSent;
        }

        // PUT: api/NotificationSents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationSent(int id, NotificationSent notificationSent)
        {
            if (id != notificationSent.NotificationSentId)
            {
                return BadRequest();
            }

            _context.Entry(notificationSent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationSentExists(id))
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

        // POST: api/NotificationSents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotificationSent>> PostNotificationSent(NotificationSent notificationSent)
        {
          if (_context.NotificationSents == null)
          {
              return Problem("Entity set 'SSDbContext.NotificationSents'  is null.");
          }
            _context.NotificationSents.Add(notificationSent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotificationSent", new { id = notificationSent.NotificationSentId }, notificationSent);
        }

        // DELETE: api/NotificationSents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationSent(int id)
        {
            if (_context.NotificationSents == null)
            {
                return NotFound();
            }
            var notificationSent = await _context.NotificationSents.FindAsync(id);
            if (notificationSent == null)
            {
                return NotFound();
            }

            _context.NotificationSents.Remove(notificationSent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationSentExists(int id)
        {
            return (_context.NotificationSents?.Any(e => e.NotificationSentId == id)).GetValueOrDefault();
        }
    }
}
