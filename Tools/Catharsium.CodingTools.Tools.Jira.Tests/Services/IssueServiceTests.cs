using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace Catharsium.CodingTools.Tools.Jira.Tests.Services;

[TestClass]
public class IssueServiceTests : TestFixture<IssueService>
{
    #region Fixture

    private static string IssueKey => "My issue key";
    private JiraIssue Issue { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        this.Issue = Substitute.For<JiraIssue>();
        this.GetDependency<IJiraClient>().GetIssue(IssueKey).Returns(Task.FromResult(this.Issue));
    }

    #endregion

    #region GetIssueByKey

    [TestMethod]
    public async Task GetIssueByKey_ReturnsIssueFromClient()
    {
        var actual = await this.Target.GetIssueByKey(IssueKey);
        Assert.AreEqual(this.Issue, actual);
    }

    #endregion
}