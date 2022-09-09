using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using saleseeker_data;

namespace saleseeker_api.Controllers
{
    [Authorize]
    [Route("api/units")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly SSDbContext _context;

        public UnitsController(SSDbContext context)
        {
            _context = context;
        }

        // GET: api/Units
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetUnits()
        {
          if (_context.Units == null)
          {
              return NotFound();
          }
            return await _context.Units.ToListAsync();
        }

        // GET: api/Units/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetUnit(int id)
        {
          if (_context.Units == null)
          {
              return NotFound();
          }
            var unit = await _context.Units.FindAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            return unit;
        }

        // PUT: api/Units/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnit(int id, Unit unit)
        {
            if (id != unit.UnitId)
            {
                return BadRequest();
            }

            _context.Entry(unit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(id))
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

        // POST: api/Units
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unit>> PostUnit(Unit unit)
        {
          if (_context.Units == null)
          {
              return Problem("Entity set 'SSDbContext.Units'  is null.");
          }
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnit", new { id = unit.UnitId }, unit);
        }

        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            if (_context.Units == null)
            {
                return NotFound();
            }
            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnitExists(int id)
        {
            return (_context.Units?.Any(e => e.UnitId == id)).GetValueOrDefault();
        }
    }
}
