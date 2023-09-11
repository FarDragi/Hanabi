using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using Lina.Database.Repositories;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Repositories;

[Repository(typeof(ITreatingRepository))]
public class TreatingRepository : BaseRepository<TreatingEntity, ulong>, ITreatingRepository
{
    public TreatingRepository(DbContext dbContext) : base(dbContext)
    {
    }
}