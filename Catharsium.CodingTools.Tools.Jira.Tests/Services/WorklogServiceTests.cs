using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.CodingTools.Tools.Jira.Tests.Services
{
    [TestClass]
    public class WorklogServiceTests : TestFixture<WorklogService>
    {
        #region Fixture

        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }

        private JiraCodingToolsSettings Settings { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            this.EndDate = DateTime.Today;
            this.StartDate = this.EndDate.AddDays(-7);

            this.Settings = new JiraCodingToolsSettings {
                Username = "My username",
                TeamMembers = new[] {
                    "My member 1",
                    "My member 2"
                }
            };
            this.SetDependency(this.Settings);
        }

        #endregion

        #region GetWorklogsForTeam

        [TestMethod]
        public async Task GetWorklogsForTeam_JiraReturnsIssues_ReturnsWorkflows()
        {
            var expectedQuery = JiraQueries.IssuesForUsersWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"))
                .Replace("{users}", string.Join(" OR ", this.Settings.TeamMembers.Select(m => $"worklogAuthor = \"{m}\"")));
            var issues = new[] { new IssueAdapter(), new IssueAdapter(), new IssueAdapter() };
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));
            var worklog1 = Substitute.For<WorklogAdapter>();
            worklog1.Author.Returns(this.Settings.TeamMembers[0]);
            worklog1.StartDate.Returns(this.EndDate);
            var worklog2 = Substitute.For<WorklogAdapter>();
            worklog2.Author.Returns(this.Settings.TeamMembers[1]);
            worklog2.StartDate.Returns(this.EndDate);
            var worklog3 = Substitute.For<WorklogAdapter>();
            worklog3.Author.Returns("Other member");
            worklog3.StartDate.Returns(this.EndDate);
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(0))
                .Returns(Task.FromResult(new[] { worklog1 }.AsEnumerable()));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(1))
                .Returns(Task.FromResult(new[] { worklog2 }.AsEnumerable()));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(2))
                .Returns(Task.FromResult(new[] { worklog3 }.AsEnumerable()));

            var actual = await this.Target.GetWorklogsForTeam(this.StartDate, this.EndDate);
            Assert.AreEqual(2, actual.Count());
            foreach (var worklog in actual) {
                Assert.IsTrue(worklog == worklog1 || worklog == worklog2);
            }
        }


        [TestMethod]
        public async Task GetWorklogsForTeam_JiraReturnsNoIssues_ReturnsEmptyList()
        {
            var expectedQuery = JiraQueries.IssuesForUsersWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"))
                .Replace("{users}", string.Join(" OR ", this.Settings.TeamMembers));
            var issues = Array.Empty<IssueAdapter>();
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));

            var actual = await this.Target.GetWorklogsForTeam(this.StartDate, this.EndDate);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        #endregion

        #region GetWorklogsForTeam

        [TestMethod]
        public async Task GetWorklogsForUser_JiraReturnsIssues_ReturnsWorkflows()
        {
            var expectedQuery = JiraQueries.IssuesForCurrentUserWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = new[] { new IssueAdapter(), new IssueAdapter() };
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));
            var worklog1 = Substitute.For<WorklogAdapter>();
            worklog1.Author.Returns(this.Settings.Username);
            worklog1.StartDate.Returns(this.EndDate);
            var worklog2 = Substitute.For<WorklogAdapter>();
            worklog2.Author.Returns("Other member");
            worklog2.StartDate.Returns(this.EndDate);
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(0))
                .Returns(Task.FromResult(new[] { worklog1 }.AsEnumerable()));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(1))
                .Returns(Task.FromResult(new[] { worklog2 }.AsEnumerable()));

            var actual = await this.Target.GetWorklogsForUser(this.StartDate, this.EndDate);
            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.First() == worklog1);
        }


        [TestMethod]
        public async Task GetWorklogsForUser_JiraReturnsNoIssues_ReturnsEmptyList()
        {
            var expectedQuery = JiraQueries.IssuesForCurrentUserWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = Array.Empty<IssueAdapter>();
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));

            var actual = await this.Target.GetWorklogsForUser(this.StartDate, this.EndDate);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        #endregion
    }
}