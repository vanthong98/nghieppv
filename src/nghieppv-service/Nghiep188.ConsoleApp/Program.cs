using Microsoft.EntityFrameworkCore;
using Nghiep188.Api.Persistence;
using Nghiep188.Api.Service;

var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("nghieppv");

var dbContext = new AppDbContext(options.Options);

var userService = new UserService(dbContext);

var rollService = new RollService(dbContext, userService);

string userName = string.Empty;
string clientSeed = string.Empty;

await Init();

var userChoice = -1;

do
{
    await PrintMasterMenuAsync();
    userChoice = int.Parse(Console.ReadLine());
    var task = userChoice switch
    {
        1 => RollAsync(),
        0 => Task.CompletedTask,
        _ => throw new NotImplementedException()
    };

    await task;
}
while (userChoice > 0);


async Task Init()
{
    Console.WriteLine("Enter your name: ");
    userName = Console.ReadLine();
    Console.WriteLine("Enter your client seed: ");
    clientSeed = Console.ReadLine();
}

async Task RollAsync()
{
    var roll = await rollService.RollAsync(userName, clientSeed, Nghiep188.Api.Enum.BetOption.Lo, 1);

    Console.WriteLine("");
    Console.WriteLine("Rolling ...");
    Console.WriteLine("");

    await Task.Delay(200);
   
    Console.WriteLine($"***********************************");
    Console.WriteLine($"Your lucky number was {roll.Number}");
    Console.WriteLine($"***********************************");
    Console.WriteLine();

    await userService.ChangeServerSeedAsync(userName);

    var serverSeeds = await userService.GetServerSeedsAsync(userName);

    var lastServerSeed = serverSeeds.FirstOrDefault(x => !x.IsActive);

    Console.WriteLine();
    Console.WriteLine($"---- LAST ROLL INFORMATION ----");
    Console.WriteLine();
    Console.WriteLine($"Last server seed is: {lastServerSeed?.Value}");
    Console.WriteLine($"Last server seed (HASHED) is: {lastServerSeed?.Sha256HashedValue}");
    Console.WriteLine($"Last nonce is: {lastServerSeed?.Nonce}");
    Console.WriteLine();

    Console.WriteLine($"Enter any key to continue...");

    Console.ReadLine();
    Console.Clear();
}

async Task PrintMasterMenuAsync()
{
    var user = await userService.GetUserAsync(userName);

    var serverSeeds = await userService.GetServerSeedsAsync(userName);

    var activeServerSeed = serverSeeds.Single(x => x.IsActive);

    activeServerSeed.HideActiveServerSeed();

    Console.WriteLine();
    Console.WriteLine($"---- NEXT ROLL INFORMATION ----");
    Console.WriteLine($"Next server seed is: {activeServerSeed.Value}");
    Console.WriteLine($"Next server seed (SHA 256 HASHED) is: {activeServerSeed.Sha256HashedValue}");
    Console.WriteLine($"Next nonce is: {activeServerSeed?.Nonce +1}");
    Console.WriteLine($"-------------------------------");
    Console.WriteLine();

    Console.WriteLine($"Hi {userName}!");
    Console.WriteLine($"Your client seed is: {clientSeed} ");
    Console.WriteLine();
    Console.WriteLine($"1. ROLL");
    Console.WriteLine($"0. EXIT");
    Console.Write($"Type a number to continue: ");
}
