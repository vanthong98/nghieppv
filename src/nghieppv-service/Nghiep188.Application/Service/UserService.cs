using Microsoft.EntityFrameworkCore;
using Nghiep188.Api.Persistence;
using Nghiep188.Api.Persistence.Entity;

namespace Nghiep188.Api.Service;

public class UserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> CreateUserAsync(string userName)
    {
        var user = new User
        {
            UserName = userName,
            Balance = 1000000000,
            CreatedDate = DateTimeOffset.Now,
        };

        var serverSeed = Helper.CreateServerSeed();

        serverSeed.User = user;

        await _dbContext.AddAsync(serverSeed);
        
        await _dbContext.SaveChangesAsync();

        return user;
    }
    
    public Task<List<ServerSeed>> GetServerSeedsAsync(string userName)
    {
        return _dbContext.ServerSeeds
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedDate)
            .Where(x => x.User!.UserName == userName)
            .ToListAsync();
    }
    
    public async Task<List<ServerSeed>> ChangeServerSeedAsync(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        
        user ??= await CreateUserAsync(userName);

        var serverSeeds = await _dbContext.ServerSeeds
            .Include(x => x.User)
            .Where(x => x.User!.UserName == userName)
            .ToListAsync();

        foreach (var serverSeed in serverSeeds)
        {
            serverSeed.IsActive = false;
        }
        
        _dbContext.UpdateRange(serverSeeds);

        var newServerSeed = Helper.CreateServerSeed();
        newServerSeed.UserId = user.Id;
        
        await _dbContext.AddAsync(newServerSeed);
        await _dbContext.SaveChangesAsync();

        return await GetServerSeedsAsync(userName);
    }

    public async Task<User> GetUserAsync(string userName)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        user ??= await CreateUserAsync(userName);
        return user;
    }
}