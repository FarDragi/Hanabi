namespace FarDragi.Hanabi.Models.Interfaces;

public interface IEventConfig
{
    public IEnumerable<ulong> Exclude { get; }
}