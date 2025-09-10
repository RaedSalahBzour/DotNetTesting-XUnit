using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserRepository repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await repository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var result = await repository.CreateAsync(user);
            if (!result)
                return BadRequest("User could not be created.");

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(User user)
        {
            var result = await repository.UpdateAsync(user);
            if (!result)
                return NotFound();

            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await repository.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}