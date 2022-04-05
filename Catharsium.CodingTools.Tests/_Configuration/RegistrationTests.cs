using Catharsium.CodingTools._Configuration;
using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.ActionHandlers.Encryption;
using Catharsium.CodingTools.ActionHandlers.Generate;
using Catharsium.CodingTools.ActionHandlers.Jira;
using Catharsium.CodingTools.ActionHandlers.Jira.Steps;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
namespace Catharsium.Util.Tests._Configuration;

[TestClass]
public class RegistrationTests
{
    [TestMethod]
    public void AddCatharsiumUtilities_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        serviceCollection.AddCodingTools(config);
        serviceCollection.ReceivedRegistration<CodingToolsSettings>();

        serviceCollection.ReceivedRegistration<IMenuActionHandler, GenerationActionHandler>();
        serviceCollection.ReceivedRegistration<IMenuActionHandler, EncryptionActionHandler>();
        serviceCollection.ReceivedRegistration<IMenuActionHandler, JiraActionHandler>();

        serviceCollection.ReceivedRegistration<IGenerationActionHandler, GuidActionHandler>();

        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraWorklogOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraSprintOverviewActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraAddWorklogActionHandler>();
        serviceCollection.ReceivedRegistration<IJiraActionHandler, JiraRemoveWorklogActionHandler>();

        serviceCollection.ReceivedRegistration<IJiraIssueSelector, JiraIssueSelector>();
        serviceCollection.ReceivedRegistration<IJiraWorklogRetriever, JiraWorklogRetriever>();

        serviceCollection.ReceivedRegistration<Atlassian.Jira.Jira>();
    }
}