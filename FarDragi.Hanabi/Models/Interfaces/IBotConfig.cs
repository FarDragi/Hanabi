namespace FarDragi.Hanabi.Models.Interfaces;

public interface IBotConfig
{
    public string Token { get; }
    public ulong OwnerGuild { get; }
    public string NiceToken { get; }
}