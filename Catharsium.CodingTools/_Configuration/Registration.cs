using Atlassian.Jira;
using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.ActionHandlers.Encryption;
using Catharsium.CodingTools.ActionHandlers.Generate;
using Catharsium.CodingTools.ActionHandlers.Jira;
using Catharsium.CodingTools.ActionHandlers.Jira.Steps;
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

        services.AddScoped<IMenuActionHandler, GenerationActionHandler>();
        services.AddScoped<IMenuActionHandler, EncryptionActionHandler>();
        services.AddScoped<IMenuActionHandler, JiraActionHandler>();

        services.AddScoped<IGenerationActionHandler, GuidActionHandler>();

        services.AddScoped<IJiraActionHandler, JiraWorklogOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraSprintOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraAddWorklogActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraRemoveWorklogActionHandler>();

        services.AddScoped<IJiraIssueSelector, JiraIssueSelector>();
        services.AddScoped<IJiraWorklogRetriever, JiraWorklogRetriever>();

        services.AddSingleton(sp => Jira.CreateRestClient(configuration.Jira.Url, configuration.Jira.Username, configuration.Jira.Password));

        return services;
    }
}