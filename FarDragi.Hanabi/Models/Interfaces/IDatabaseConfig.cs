using Config.Net;

namespace FarDragi.Hanabi.Models.Interfaces;

public interface IDatabaseConfig
{
    [Option(DefaultValue = "Server=localhost;Database=hanabi;User Id=root;Password=root;")]
    public string Url { get; }
}