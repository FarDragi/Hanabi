using FarDragi.Hanabi.Models.Interfaces;
using Lina.Database;
using Lina.Database.Context;
using Lina.LoaderConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace FarDragi.Hanabi.Database;

public class DesignContextFactory : IDesignTimeDbContextFactory<LinaDbContext>
{
    public LinaDbContext CreateDbContext(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        var config = serviceCollection.AddLoaderConfig<IAppConfig>();
        serviceCollection.AddLinaDbContext<Program>((builder, assembly) => builder.UseMySql(config.Database.Url,
            ServerVersion.AutoDetect(config.Database.Url), options => options.MigrationsAssembly(assembly)));

        var services = serviceCollection.BuildServiceProvider();

        return services.GetRequiredService<LinaDbContext>();
    }
}