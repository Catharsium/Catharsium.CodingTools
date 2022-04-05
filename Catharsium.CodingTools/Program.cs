using Catharsium.CodingTools._Configuration;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.CodingTools;

class Program
{
    static async Task Main(string[] args)
    {
        var appsettingsFilePath = @"appsettings.json";
        if (args.Length > 0) {
            appsettingsFilePath = args[0];
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appsettingsFilePath, false, false);
        var configuration = builder.Build();

        var serviceProvider = new ServiceCollection()
            .AddCodingTools(configuration)
            .BuildServiceProvider();

        var mainMenuActionHandler = serviceProvider.GetService<IMainMenuActionHandler>();
        if (mainMenuActionHandler != null) {
            await mainMenuActionHandler.Run();
        }
    }
}