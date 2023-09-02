using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using Lina.Database.Repositories;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Repositories;

[Repository(typeof(IInviteRepository))]
public class InviteRepository : BaseRepository<InviteEntity, string>, IInviteRepository
{
    public InviteRepository(DbContext dbContext) : base(dbContext)
    {
    }
}