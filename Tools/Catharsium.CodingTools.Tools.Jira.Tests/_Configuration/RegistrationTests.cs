using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;
using Catharsium.CodingTools.Tools.Jira.Client;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
namespace Catharsium.CodingTools.Tools.Jira.Tests._Configuration;

[TestClass]
public class RegistrationTests
{
    [TestMethod]
    public void AddCatharsiumUtilities_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        serviceCollection.AddJiraCodingTools(config);
        serviceCollection.ReceivedRegistration<Atlassian.Jira.Jira>();
        serviceCollection.ReceivedRegistration<IJiraClient, JiraClient>();

        serviceCollection.ReceivedRegistration<IJiraActionHandler, PersonalWeekOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, TeamWeekOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, AddWorklogActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, RemoveWorklogActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, LabelReportActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, HoursReportActionHandler>();

        serviceCollection.ReceivedRegistration<IJiraIssueSelector, JiraIssueSelector>();
        serviceCollection.ReceivedRegistration<IPeriodSelector, PeriodSelector>();

        serviceCollection.ReceivedRegistration<ICsvFileService, CsvFileService>();
        serviceCollection.ReceivedRegistration<IIssueService, IssueService>();
        serviceCollection.ReceivedRegistration<IWorklogService, WorklogService>();
        serviceCollection.ReceivedRegistration<ITimesheetService, TimesheetService>();
    }
}