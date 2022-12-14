using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using saleseeker_data;

namespace saleseeker_api.Controllers
{
    [Authorize]
    [Route("api/packs")]
    [ApiController]
    public class PacksController : ControllerBase
    {
        private readonly SSDbContext _context;

        public PacksController(SSDbContext context)
        {
            _context = context;
        }

        // GET: api/Packs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pack>>> GetPacks()
        {
          if (_context.Packs == null)
          {
              return NotFound();
          }
            return await _context.Packs.ToListAsync();
        }

        // GET: api/Packs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pack>> GetPack(int id)
        {
          if (_context.Packs == null)
          {
              return NotFound();
          }
            var pack = await _context.Packs.FindAsync(id);

            if (pack == null)
            {
                return NotFound();
            }

            return pack;
        }

        // PUT: api/Packs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPack(int id, Pack pack)
        {
            if (id != pack.PackId)
            {
                return BadRequest();
            }

            _context.Entry(pack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackExists(id))
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

        // POST: api/Packs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pack>> PostPack(Pack pack)
        {
          if (_context.Packs == null)
          {
              return Problem("Entity set 'SSDbContext.Packs'  is null.");
          }
            _context.Packs.Add(pack);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPack", new { id = pack.PackId }, pack);
        }

        // DELETE: api/Packs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePack(int id)
        {
            if (_context.Packs == null)
            {
                return NotFound();
            }
            var pack = await _context.Packs.FindAsync(id);
            if (pack == null)
            {
                return NotFound();
            }

            _context.Packs.Remove(pack);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PackExists(int id)
        {
            return (_context.Packs?.Any(e => e.PackId == id)).GetValueOrDefault();
        }
    }
}
