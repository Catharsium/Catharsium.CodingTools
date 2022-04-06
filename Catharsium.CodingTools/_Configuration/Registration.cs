using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.ActionHandlers.Encryption;
using Catharsium.CodingTools.ActionHandlers.Generate;
using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.CodingTools._Configuration;

public static class Registration
{
    public static IServiceCollection AddCodingTools(this IServiceCollection services, IConfiguration config)
    {
        var configuration = config.Load<CodingToolsSettings>();
        services.AddSingleton<CodingToolsSettings, CodingToolsSettings>(provider => configuration);

        services.AddConsoleIoUtilities(config);

        services.AddJiraCodingTools(config);

        services.AddScoped<IMenuActionHandler, GenerationActionHandler>();
        services.AddScoped<IMenuActionHandler, EncryptionActionHandler>();

        services.AddScoped<IGenerationActionHandler, GuidActionHandler>();

        services.AddScoped<IEncryptionActionHandler, Sha256EncryptionActionHandler>();

        return services;
    }
}