using Catharsium.CodingTools.Tools.Jira.ActionHandlers;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;
using Catharsium.CodingTools.Tools.Jira.Client;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models.Mappers;
using Catharsium.CodingTools.Tools.Jira.Services;
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
        return services.AddSingleton<JiraCodingToolsSettings, JiraCodingToolsSettings>(provider => configuration)
            .AddConsoleIoUtilities(config)
            .AddFilesIoUtilities(config)

            .AddSingleton(sp => Atlassian.Jira.Jira.CreateRestClient(configuration.Url, configuration.Username, configuration.Password))
            .AddScoped<IJiraClient, JiraClient>()
            .AddScoped<IJiraIssueMapper, JiraIssueMapper>()
            .AddScoped<IWorklogMapper, WorklogMapper>()

            .AddScoped<IJiraActionHandler, PersonalWeekOverviewActionHandler>()
            .AddScoped<IJiraActionHandler, TeamWeekOverviewActionHandler>()
            .AddScoped<IJiraActionHandler, AddWorklogActionHandler>()
            .AddScoped<IJiraActionHandler, RemoveWorklogActionHandler>()
            .AddScoped<IJiraActionHandler, LabelReportActionHandler>()
            .AddScoped<IJiraActionHandler, HoursReportActionHandler>()

            .AddScoped<IJiraIssueSelector, JiraIssueSelector>()
            .AddScoped<IPeriodSelector, PeriodSelector>()

            .AddScoped<ICsvFileService, CsvFileService>()
            .AddScoped<IIssueService, IssueService>()
            .AddScoped<IWorklogService, WorklogService>()
            .AddScoped<ITimesheetService, TimesheetService>();
    }
}