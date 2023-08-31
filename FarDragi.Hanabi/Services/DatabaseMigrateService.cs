using FarDragi.Hanabi.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.EntityFrameworkCore;

namespace FarDragi.Hanabi.Services;

[Service(typeof(IDatabaseMigrateService))]
public class DatabaseMigrateService : IDatabaseMigrateService
{
    private readonly DbContext _context;

    public DatabaseMigrateService(DbContext context)
    {
        _context = context;
    }

    public async Task Migrate()
    {
        await _context.Database.MigrateAsync();
    }
}