namespace FarDragi.Hanabi.Models.Interfaces;

public interface IAppConfig
{
    public IBotConfig Bot { get; }
    public IChannelsConfig Channels { get; set; }
}