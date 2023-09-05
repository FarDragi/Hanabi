using FarDragi.Hanabi.Models;

namespace FarDragi.Hanabi.Services.Interfaces;

public interface ICandyService
{
    Task<CandyDto> AddCandies(CandyDto dto);
    bool IsCandyEvent();
}