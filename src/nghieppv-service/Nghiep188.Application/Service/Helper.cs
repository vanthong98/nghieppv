using System.Security.Cryptography;
using System.Text;
using Nghiep188.Api.Persistence.Entity;

namespace Nghiep188.Api.Service;

public abstract class Helper
{
    public static ServerSeed CreateServerSeed()
    {
        var randomInt = new Random().Next();

        var serverSeedValue = GetSha256(randomInt.ToString());
        var serverSeedHash = GetSha256(serverSeedValue);
        
        var serverSeed = new ServerSeed
        {
            Value = serverSeedValue,
            Sha256HashedValue = serverSeedHash,
            CreatedDate = DateTimeOffset.Now,
            IsActive = true
        };

        return serverSeed;
    }
    
    public static int Roll(string serverSeed, string clientSeed, int nonce)
    {
        var hex = GetHmac(serverSeed, clientSeed, nonce);
        var index = 0;

        var lucky = GetNumber(hex, index);

        while (lucky >= 1e6)
        {
            index += 1;
            lucky = GetNumber(hex, index);
            if (index * 5 + 5 <= 129)
            {
                continue;
            }

            lucky = 9999;
            break;
        }

        var result = lucky % 1e4;

        return (int)Math.Round(result);
    }

    private static int GetNumber(string hex, int index)
    {
        var substring = hex.Substring(index * 5, 5);
        return int.Parse(substring, System.Globalization.NumberStyles.HexNumber);
    }

    private static string GetHmac(string serverSeed, string clientSeed, int nonce)
    {
        using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(serverSeed));
        var hash = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes($"{clientSeed}-{nonce}"));
        return ToHexString(hash);
    }

    private static string GetSha256(string value)
    {
        var data = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(data);
        return ToHexString(hash);
    }

    private static string ToHexString(byte[] bytes)
    {
        var hex = BitConverter.ToString(bytes);
        return hex.Replace("-", "");
    }
}