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
  public class ProfilesController : ControllerBase
  {
    private readonly LoggerDBContext _context;

    public ProfilesController(LoggerDBContext context)
    {
      _context = context;
    }

    // GET: api/Profiles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profile>>> GetProfile()
    {
      return await _context.Profile.ToListAsync();
    }

    // GET: api/Profiles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Profile>> GetProfile(int id)
    {
      Profile profile = await _context.Profile.FindAsync(id);

      if (profile == null)
      {
        return NotFound();
      }

      return profile;
    }

    // PUT: api/Profiles/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfile(int id, Profile profile)
    {
      if (id != profile.Id)
      {
        return BadRequest();
      }

      _context.Entry(profile).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProfileExists(id))
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

    // POST: api/Profiles
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPost]
    public async Task<ActionResult<Profile>> PostProfile(Profile profile)
    {

      if (profile.Id > 0)
      {
        ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails
        {
          Title = ""
        };

        return ValidationProblem(validationProblemDetails);
      }

      if (_context.Profile.Any(x => x.EmpId == profile.EmpId))
      {
        ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails
        {
          Title = TimeLogger.Web.Resources.ErrorEmpId,
        };

        return ValidationProblem(validationProblemDetails);
      }

      _context.Profile.Add(profile);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
    }

    // DELETE: api/Profiles/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Profile>> DeleteProfile(int id)
    {
      Profile profile = await _context.Profile.FindAsync(id);
      if (profile == null)
      {
        return NotFound();
      }

      _context.Profile.Remove(profile);
      await _context.SaveChangesAsync();

      return profile;
    }

    private bool ProfileExists(int id)
    {
      return _context.Profile.Any(e => e.Id == id);
    }
  }
}
