using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using saleseeker_data;

namespace saleseeker_api.Controllers
{
    [Authorize]
    [Route("api/subscribedusers")]
    [ApiController]
    public class SubscribedUsersController : ControllerBase
    {
        private readonly SSDbContext _context;

        public SubscribedUsersController(SSDbContext context)
        {
            _context = context;
        }

        #region CRUD

        // GET: api/SubscribedUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscribedUser>>> GetSubscribedUsers()
        {
          if (_context.SubscribedUsers == null)
          {
              return NotFound();
          }
            return await _context.SubscribedUsers.ToListAsync();
        }

        // GET: api/SubscribedUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscribedUser>> GetSubscribedUser(int id)
        {
          if (_context.SubscribedUsers == null)
          {
              return NotFound();
          }
            var subscribedUser = await _context.SubscribedUsers.FindAsync(id);

            if (subscribedUser == null)
            {
                return NotFound();
            }

            return subscribedUser;
        }

        // PUT: api/SubscribedUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscribedUser(int id, SubscribedUser subscribedUser)
        {
            if (id != subscribedUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(subscribedUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscribedUserExists(id))
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

        // POST: api/SubscribedUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscribedUser>> PostSubscribedUser(SubscribedUser subscribedUser)
        {
          if (_context.SubscribedUsers == null)
          {
              return Problem("Entity set 'SSDbContext.SubscribedUsers'  is null.");
          }
            _context.SubscribedUsers.Add(subscribedUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscribedUser", new { id = subscribedUser.UserId }, subscribedUser);
        }

        // DELETE: api/SubscribedUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscribedUser(int id)
        {
            if (_context.SubscribedUsers == null)
            {
                return NotFound();
            }
            var subscribedUser = await _context.SubscribedUsers.FindAsync(id);
            if (subscribedUser == null)
            {
                return NotFound();
            }

            _context.SubscribedUsers.Remove(subscribedUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscribedUserExists(int id)
        {
            return (_context.SubscribedUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        #endregion

        #region other

        //// GET: api/SubscribedUsers/Item/5
        //[HttpGet("item/{id}")]
        //public async Task<ActionResult<IEnumerable<SubscribedUser>>> GetSubscribedUsersForItem(int id)
        //{
        //    if (_context.SubscribedUsers == null)
        //    {
        //        return NotFound();
        //    }

        //    var item = await _context.Items.FindAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    var subscribedItems = _context.SubscribedItems.Where(a=>a.ItemId == id);

        //    if (subscribedItems.Count() == 0)
        //    {
        //        return NotFound();
        //    }

        //    return subscribedItems.SelectMany(a => a.SubscribedUser);
        //}

        #endregion
    }
}
