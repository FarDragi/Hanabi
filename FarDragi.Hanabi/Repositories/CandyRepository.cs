using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using Lina.Database.Repositories;
using Lina.DynamicServicesProvider.Attributes;
using Lina.UtilsExtensions;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Repositories;

[Repository(typeof(ICandyRepository))]
public class CandyRepository : BaseRepository<CandyEntity, ulong>, ICandyRepository
{
    private readonly DbContext _dbContext;

    public CandyRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CandyEntity>> GetLeaderBoard(int page, int pageSize)
    {
        return await _dbContext.Set<CandyEntity>().Paginate(page, pageSize).ToListAsync();
    }
}