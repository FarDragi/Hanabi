using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class HalloweenEntity
{
    public static bool IsHalloween()
    {
#if DEBUG
        return true;
#else
        return DateTimeOffset.Now.Month == 10;
#endif
    }
}