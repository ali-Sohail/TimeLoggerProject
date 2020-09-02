using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeLogger.Web.Models;
using TimeLogger.Web.Services;

namespace TimeLogger.Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController] 
	public class UsersController : ControllerBase
	{
		private readonly LoggerDBContext _context;

		//public UsersController(LoggerDBContext context)
		//{
		//	_context = context;
		//}

		private IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}


		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody]User model)
		{
			if (model != null 
				&& !string.IsNullOrEmpty(model.Username) 
				&& !string.IsNullOrEmpty(model.Password))
			{
				var profile = _userService.Authenticate(model.Username, model.Password);

				if (profile == null)
					return BadRequest(new { message = "Username or password is incorrect" });

				return Ok(profile);
			}
			return Problem(detail: "Username or password is incorrect");
		}	


		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUser()
		{
			return await _context.User.ToListAsync();
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _context.User.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int id, User user)
		{
			if (id != user.UserId)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
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

		// POST: api/Users
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			_context.User.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.UserId }, user);
		}

		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<User>> DeleteUser(int id)
		{
			var user = await _context.User.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.User.Remove(user);
			await _context.SaveChangesAsync();

			return user;
		}

		private bool UserExists(int id)
		{
			return _context.User.Any(e => e.UserId == id);
		}
	}
}