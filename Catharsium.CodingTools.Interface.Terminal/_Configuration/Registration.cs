using Catharsium.CodingTools.Interface.Terminal.ActionHandlers;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Generate;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.CodingTools.Interface.Terminal._Configuration;

public static class Registration
{
    public static IServiceCollection AddCodingToolsTerminal(this IServiceCollection services, IConfiguration config)
    {
        var configuration = config.Load<CodingToolsTerminalSettings>();
        services.AddSingleton<CodingToolsTerminalSettings, CodingToolsTerminalSettings>(provider => configuration);

        services.AddConsoleIoUtilities(config);

        services.AddScoped<IMenuActionHandler, GenerationActionHandler>();
        services.AddScoped<IMenuActionHandler, EncryptionActionHandler>();
        services.AddScoped<IMenuActionHandler, JiraActionHandler>();

        services.AddScoped<IGenerationActionHandler, GuidActionHandler>();

        services.AddScoped<IJiraActionHandler, JiraWorklogOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraAddWorklogActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraRemoveWorklogActionHandler>();

        return services;
    }
}