using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeLogger.Web.Models;

namespace TimeLogger.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DayLogsController : ControllerBase
  {
    private readonly LoggerDBContext _context;

    public DayLogsController(LoggerDBContext context)
    {
      _context = context;
    }

    // GET: api/DayLogs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DayLog>>> GetDayLog()
    {
      return await _context.DayLog.ToListAsync();
    }

    // GET: api/DayLogs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DayLog>> GetDayLog(int id)
    {
      DayLog dayLog = await _context.DayLog.FindAsync(id);

      if (dayLog == null)
      {
        return NotFound();
      }
      return dayLog;
    }

    // PUT: api/DayLogs/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDayLog(int id, DayLog dayLog)
    {
      if (id != dayLog.Id)
      {
        return BadRequest();
      }

      _context.Entry(dayLog).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!DayLogExists(id))
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

    // POST: api/DayLogs
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPost]
    public async Task<ActionResult<DayLog>> PostDayLog(DayLog dayLog)
    {
      if (_context.Profile.Any(x => x.EmpId == dayLog.UserId))
      {
        _context.DayLog.Add(dayLog);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDayLog", new { id = dayLog.Id }, dayLog);
      }
      return NotFound();
    }

    // DELETE: api/DayLogs/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<DayLog>> DeleteDayLog(int id)
    {
      DayLog dayLog = await _context.DayLog.FindAsync(id);
      if (dayLog == null)
      {
        return NotFound();
      }

      _context.DayLog.Remove(dayLog);
      await _context.SaveChangesAsync();

      return dayLog;
    }

    private bool DayLogExists(int id)
    {
      return _context.DayLog.Any(e => e.Id == id);
    }
  }
}
