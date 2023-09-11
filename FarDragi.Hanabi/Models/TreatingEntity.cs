using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class TreatingEntity : BaseEntity<ulong>
{
    public TreatingEntity(ulong id, int count)
    {
        Id = id;
        Count = count;
    }

    public int Count { get; set; }
}