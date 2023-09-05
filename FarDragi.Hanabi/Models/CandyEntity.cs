using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class CandyEntity : BaseEntity<ulong>
{
    public CandyEntity(ulong id, int count)
    {
        Id = id;
        Count = count;
    }

    public int Count { get; set; }
}