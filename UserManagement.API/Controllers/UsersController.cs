using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Models;
using UserManagement.API.Services;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(UserStore store) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<User>> GetAll() => Ok(store.All());

    [HttpGet("{id:int}")]
    public ActionResult<User> Get(int id)
    {
        var user = store.Get(id);
        return user is null ? NotFound(new { message = "User not found." }) : Ok(user);
    }

    [HttpPost]
    public ActionResult<User> Create([FromBody] UserInput input)
    {
        var (user, duplicate) = store.Add(input);
        if (duplicate) return Conflict(new { message = "Email already exists." });
        return CreatedAtAction(nameof(Get), new { id = user!.Id }, user);
    }

    [HttpPut("{id:int}")]
    public ActionResult<User> Update(int id, [FromBody] UserInput input)
    {
        var (user, duplicate) = store.Update(id, input);
        if (duplicate) return Conflict(new { message = "Email already exists." });
        return user is null ? NotFound(new { message = "User not found." }) : Ok(user);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) => store.Delete(id) ? NoContent() : NotFound(new { message = "User not found." });
}
