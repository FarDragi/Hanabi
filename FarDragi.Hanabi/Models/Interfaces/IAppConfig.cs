namespace FarDragi.Hanabi.Models.Interfaces;

public interface IAppConfig
{
    public IBotConfig Bot { get; }
    public IChannelsConfig Channels { get; }
    public IDatabaseConfig Database { get; }
    public IEventConfig Event { get; }
}