using FarDragi.Hanabi.Models;
using Lina.Database.Interfaces;

namespace FarDragi.Hanabi.Repositories.Interfaces;

public interface ICandyRepository : IBaseRepository<CandyEntity, ulong>
{
    Task<IEnumerable<CandyEntity>> GetLeaderBoard(int page, int pageSize);
}