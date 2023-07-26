using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.CodingTools.Tools.Jira.Tests.Services;

[TestClass]
public class IssueServiceTests : TestFixture<IssueService>
{
    #region Fixture

    private static string IssueKey => "My issue key";
    private IssueAdapter Issue { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        this.Issue = Substitute.For<IssueAdapter>();
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

    #region GetEpicForIssue

    [TestMethod]
    public async Task GetEpicForIssue_NoEpicLinkField_ReturnsNull()
    {
        var actual = await this.Target.GetEpicForIssue(this.Issue);
        Assert.IsNull(actual);
    }


    [TestMethod]
    public async Task GetEpicForIssue_EmptyEpicLinkField_ReturnsNull()
    {
        this.Issue.GetCustomFields().Returns(new Dictionary<string, string[]> { { CustomFields.EpicLink, Array.Empty<string>() } });
        var actual = await this.Target.GetEpicForIssue(this.Issue);
        Assert.IsNull(actual);
    }


    [TestMethod]
    public async Task GetEpicForIssue_ExistingEpicLink_ReturnsEpicIssue()
    {
        var epicLink = "My epic link";
        this.Issue.GetCustomFields().Returns(new Dictionary<string, string[]> { { CustomFields.EpicLink, new string[] { epicLink } } });

        var expected = Substitute.For<IssueAdapter>();
        this.GetDependency<IJiraClient>().GetIssue(epicLink).Returns(Task.FromResult(expected));

        var actual = await this.Target.GetEpicForIssue(this.Issue);
        Assert.AreEqual(expected, actual);
    }

    #endregion
}