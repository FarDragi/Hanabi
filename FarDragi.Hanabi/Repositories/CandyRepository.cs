using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using Lina.Database.Repositories;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Repositories;

[Repository(typeof(ICandyRepository))]
public class CandyRepository : BaseRepository<CandyEntity, ulong>, ICandyRepository
{
    public CandyRepository(DbContext dbContext) : base(dbContext)
    {
    }
}