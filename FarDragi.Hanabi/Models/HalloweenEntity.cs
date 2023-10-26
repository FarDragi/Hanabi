using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class HalloweenEntity
{
    public static bool IsHalloween()
    {
#if DEBUG
        return true;
#else
        var now = DateTimeOffset.Now;
        now = now.AddHours(-3);
        return now.Month == 10;
#endif
    }
}