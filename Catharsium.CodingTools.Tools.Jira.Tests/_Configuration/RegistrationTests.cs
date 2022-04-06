using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
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
        serviceCollection.ReceivedRegistration<IMenuActionHandler, JiraActionHandler>();

        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraWorklogOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraSprintOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraAddWorklogActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraRemoveWorklogActionHandler>();

        serviceCollection.ReceivedRegistration<IJiraIssueSelector, JiraIssueSelector>();

        serviceCollection.ReceivedRegistration<IJiraWorklogService, JiraWorklogService>();
        serviceCollection.ReceivedRegistration<ITimesheetService, TimesheetService>();

        serviceCollection.ReceivedRegistration<Atlassian.Jira.Jira>();
    }
}