using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Repositories.Interfaces;
using Lina.Database.Repositories;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Repositories;

[Repository(typeof(IEventRepository))]
public class EventRepository : BaseRepository<EventEntity, uint>, IEventRepository
{
    public EventRepository(DbContext dbContext) : base(dbContext)
    {
    }
}