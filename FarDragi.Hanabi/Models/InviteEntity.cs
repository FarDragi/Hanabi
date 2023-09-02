using Lina.Database.Models;

namespace FarDragi.Hanabi.Models;

public class InviteEntity : BaseEntity<string>
{
    public InviteEntity(string id, int uses, ulong userId)
    {
        Id = id;
        Uses = uses;
        UserId = userId;
    }

    public int Uses { get; set; }
    public ulong UserId { get; set; }
}