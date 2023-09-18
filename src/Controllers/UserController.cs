using Microsoft.AspNetCore.Mvc;
using OpenTelemetryTest.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenTelemetryTest.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private AppDbContext _appDbContext;

    public UserController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _appDbContext.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    public IActionResult Post(User user)
    {
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, User user)
    {
        var userFound = _appDbContext.Users.FirstOrDefault(u => u.Id == id);
        if (userFound is null)
            return NotFound();

        userFound.Name = user.Name;
        userFound.Birthday = user.Birthday;
        _appDbContext.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var userFound = _appDbContext.Users.FirstOrDefault(u => u.Id == id);
        if (userFound is null)
            return NotFound();

        _appDbContext.Users.Remove(userFound);
        _appDbContext.SaveChanges();

        return Ok();
    }
}