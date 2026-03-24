using System.Security.Cryptography;
using System.Text;

namespace AiGarden.BuildingBlocks.Web;

public static class DeterministicGuid
{
    public static Guid Create(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        Span<byte> guidBytes = stackalloc byte[16];
        bytes[..16].CopyTo(guidBytes);
        return new Guid(guidBytes);
    }
}
