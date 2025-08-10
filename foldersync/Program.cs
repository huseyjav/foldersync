using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using foldersync.config;
using foldersync.customLoggers;


class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<Config, ConsoleConfig>();

        services.AddSingleton<foldersync.tracker.FolderTracker>(sp =>
            new foldersync.tracker.FolderTracker(sp.GetRequiredService<Config>().sourcePath));

        services.AddSingleton<foldersync.tracker.ShaTracker>(sp =>
            new foldersync.tracker.ShaTracker(sp.GetRequiredService<Config>().sourcePath));

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddCustomConsoleLogger();
            builder.AddFileLogger("outputlog");
        });

        services.AddSingleton<foldersync.sync.Synchronizer>(sp =>
            new foldersync.sync.Synchronizer(
                sp.GetRequiredService<foldersync.tracker.FolderTracker>(),
                sp.GetRequiredService<foldersync.tracker.ShaTracker>(),
                sp.GetRequiredService<Config>().sourcePath,
                sp.GetRequiredService<Config>().replicaPath,
                sp.GetRequiredService<ILogger<foldersync.sync.Synchronizer>>()
            ));

        var provider = services.BuildServiceProvider();

        var syncer = provider.GetRequiredService<foldersync.sync.Synchronizer>();
        var config1 = provider.GetRequiredService<Config>();

        while (true)
        {
            syncer.Sync();
            Thread.Sleep(config1.syncInterval);
        }
    }
}
