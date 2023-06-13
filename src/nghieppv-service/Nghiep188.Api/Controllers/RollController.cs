using Microsoft.AspNetCore.Mvc;
using Nghiep188.Api.Enum;
using Nghiep188.Api.Service;

namespace Nghiep188.Api.Controllers;

[ApiController]
[Route("rolls")]
public class RollController : ControllerBase
{
    private readonly RollService _rollService;

    public RollController(RollService rollService)
    {
        _rollService = rollService;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string userName)
    {
        var rolls = await _rollService.SearchAsync(userName);
        rolls.ForEach(x => { if (x.ServerSeed != null) x.ServerSeed!.HideActiveServerSeed(); });
        return Ok(rolls);
    }

    [HttpPost]
    public async Task<IActionResult> Roll(string userName, string clientSeed, BetOption betOption, long betAmount, long multiple)
    {
        var roll = await _rollService.RollAsync(userName, clientSeed, betOption, betAmount);

        if (roll.ServerSeed != null)
        {
            roll.ServerSeed!.HideActiveServerSeed();
        }
        return Ok(roll);
    }
}