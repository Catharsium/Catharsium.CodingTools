using Catharsium.CodingTools.Tools.Jira.ActionHandlers;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;
using Catharsium.CodingTools.Tools.Jira.Client;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Catharsium.Util.IO.Files._Configuration;
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
        services.AddFilesIoUtilities(config);

        services.AddSingleton(sp => Atlassian.Jira.Jira.CreateRestClient(configuration.Url, configuration.Username, configuration.Password));
        services.AddScoped<IJiraClient, JiraClient>();

        services.AddScoped<IJiraActionHandler, WorklogOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, AddWorklogActionHandler>();
        services.AddScoped<IJiraActionHandler, RemoveWorklogActionHandler>();
        services.AddScoped<IJiraActionHandler, TeamOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, SprintOverviewActionHandler>();
        services.AddScoped<IJiraActionHandler, LabelReportActionHandler>();
        services.AddScoped<IJiraActionHandler, HoursReport>();

        services.AddScoped<IJiraIssueSelector, JiraIssueSelector>();

        services.AddScoped<IWorklogService, WorklogService>();
        services.AddScoped<ITimesheetService, TimesheetService>();

        return services;
    }
}