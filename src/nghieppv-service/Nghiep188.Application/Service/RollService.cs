using Microsoft.EntityFrameworkCore;
using Nghiep188.Api.Enum;
using Nghiep188.Api.Exception;
using Nghiep188.Api.Persistence;
using Nghiep188.Api.Persistence.Entity;
namespace Nghiep188.Api.Service;

public class RollService
{
    private readonly AppDbContext _dbContext;
    private readonly UserService _userService;

    public RollService(AppDbContext dbContext, UserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _dbContext.Database.EnsureCreated();
    }

    public async Task<Roll> RollAsync(string userName, string clientSeed, BetOption betOption, long betAmount)
    {
        var user = await _userService.GetUserAsync(userName);

        var activeServerSeed = await _dbContext.ServerSeeds
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User!.UserName == userName && x.IsActive);

        if (activeServerSeed == null)
        {
            throw new NghiepException();
        }

        var serverSeed = activeServerSeed.Value;
        var nonce = activeServerSeed.Nonce;

        if (serverSeed == null)
        {
            throw new NghiepException();
        }

        nonce++;

        activeServerSeed.Nonce = nonce;

        var rollNumber = Helper.Roll(serverSeed, clientSeed, nonce);

        var winningOption = rollNumber < 4950 ? BetOption.Lo : BetOption.Hi;
        var win = betOption == winningOption;
        var balanceBefore = user.Balance;
        var changeAmount = win ? betAmount : -betAmount;
        var balanceAfter = balanceBefore + changeAmount;

        user.Balance = balanceAfter;

        var roll = new Roll
        {
            ServerSeedId = activeServerSeed.Id,
            Number = rollNumber,
            Option = betOption,
            BetAmount = betAmount,
            BalanceBefore = balanceBefore,
            BalanceAfter = balanceAfter,
            ClientSeed = clientSeed,
            Nonce = nonce,
            CreatedDate = DateTimeOffset.Now
        };

        _dbContext.Add(roll);
        _dbContext.Update(user);
        _dbContext.Update(activeServerSeed);

        await _dbContext.SaveChangesAsync();

        return roll;
    }

    public async Task<List<Roll>> SearchAsync(string userName)
    {
        var rolls = await _dbContext.Rolls
            .Include(x => x.ServerSeed!.User)
            .Where(x => x.ServerSeed!.User!.UserName == userName)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();

        return rolls;
    }
}