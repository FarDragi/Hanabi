using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class EventEntity : BaseEntity<uint>
{
    public EventEntity(string name, DateTimeOffset start, DateTimeOffset end)
    {
        Name = name;
        Start = start;
        End = end;
    }

    public string Name { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}