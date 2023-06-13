using Microsoft.AspNetCore.Mvc;
using Nghiep188.Api.Service;

namespace Nghiep188.Api.Controllers;

[ApiController]
[Route("users")]


public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userName}")]
    public async Task<IActionResult> GetUserInfo(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            userName = "nghieppv";
        }

        var user = await _userService.GetUserAsync(userName);

        return Ok(user);
    }
    
    [HttpGet("server-seeds")]
    public async Task<IActionResult> GetServerSeeds(string userName)
    {
        var serverSeeds = await _userService.GetServerSeedsAsync(userName);
        serverSeeds.ForEach(x => x.HideActiveServerSeed());
        return Ok(serverSeeds);
    }
    
    [HttpPost("change-server-seed")]
    public async Task<IActionResult> ChangeServerSeed(string userName)
    {
        var serverSeeds = await _userService.ChangeServerSeedAsync(userName);
        serverSeeds.ForEach(x => x.HideActiveServerSeed());
        return Ok(serverSeeds);
    }
}