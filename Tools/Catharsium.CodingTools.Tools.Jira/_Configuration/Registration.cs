using Catharsium.CodingTools.Tools.Jira.ActionHandlers;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.CodingTools.Tools.Jira._Configuration;

public static class Registration
{
    public static IServiceCollection AddJiraCodingTools(this IServiceCollection services, IConfiguration config)
    {
        var configuration = config.Load<JiraCodingToolsSettings>();
        services.AddSingleton<JiraCodingToolsSettings, JiraCodingToolsSettings>(provider => configuration);

        services.AddConsoleIoUtilities(config);

        services.AddScoped<IMenuActionHandler, JiraActionHandler>();

        services.AddScoped<IJiraActionHandler, JiraWorklogOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraSprintOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraAddWorklogActionHandler>();
        services.AddScoped<IJiraActionHandler, JiraRemoveWorklogActionHandler>();

        services.AddScoped<IJiraIssueSelector, JiraIssueSelector>();
        services.AddScoped<IJiraWorklogRetriever, JiraWorklogService>();

        services.AddSingleton(sp => Atlassian.Jira.Jira.CreateRestClient(configuration.Url, configuration.Username, configuration.Password));

        return services;
    }
}