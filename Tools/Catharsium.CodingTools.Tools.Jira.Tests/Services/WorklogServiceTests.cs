using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.CodingTools.Tools.Jira.Services;
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
        public List<WorklogAdapter> Worklogs { get; private set; }


        [TestInitialize]
        public void Initialize()
        {
            this.EndDate = DateTime.Today;
            this.StartDate = this.EndDate.AddDays(-7);

            this.Settings = new JiraCodingToolsSettings {
                Username = "My username"
            };
            this.SetDependency(this.Settings);

            var worklog1 = Substitute.For<WorklogAdapter>();
            worklog1.Author.Returns(this.Settings.Username);
            worklog1.StartDate.Returns(this.StartDate.AddDays(-1));

            var worklog2 = Substitute.For<WorklogAdapter>();
            worklog2.Author.Returns(this.Settings.Username);
            worklog2.StartDate.Returns(this.EndDate);

            var worklog3 = Substitute.For<WorklogAdapter>();
            worklog3.Author.Returns(this.Settings.Username);
            worklog3.StartDate.Returns(this.EndDate.AddDays(1));

            var worklog4 = Substitute.For<WorklogAdapter>();
            worklog4.Author.Returns(this.Settings.Username + "Other");
            worklog4.StartDate.Returns(this.EndDate);
            this.Worklogs = new List<WorklogAdapter> { worklog1, worklog2, worklog3, worklog4 };
        }

        #endregion

        #region GetWorklogsInPeriod

        [TestMethod]
        public async Task GetWorklogsInPeriod_JiraReturnsIssues_ReturnsWorklogs()
        {
            var expectedQuery = JiraQueries.IssuesWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = new[] { new IssueAdapter(), new IssueAdapter() };
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(0))
                .Returns(Task.FromResult(new[] { this.Worklogs[1] }.AsEnumerable()));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(1))
                .Returns(Task.FromResult(new[] { this.Worklogs[3] }.AsEnumerable()));

            var actual = (await this.Target.GetWorklogsInPeriod(this.StartDate, this.EndDate)).ToArray();
            Assert.AreEqual(2, actual.Length);
            foreach (var worklog in actual) {
                Assert.IsTrue(worklog == this.Worklogs[1] || worklog == this.Worklogs[3]);
            }
        }


        [TestMethod]
        public async Task GetWorklogsInPeriod_JiraReturnsNoIssues_ReturnsEmptyList()
        {
            var expectedQuery = JiraQueries.IssuesWithWorklogsInPeriod
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = Array.Empty<IssueAdapter>();
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));

            var actual = await this.Target.GetWorklogsInPeriod(this.StartDate, this.EndDate);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        #endregion

        #region GetWorklogsInPeriodForUser

        [TestMethod]
        public async Task GetWorklogsInPeriodForUser_JiraReturnsIssues_ReturnsWorkflows()
        {
            var expectedQuery = JiraQueries.IssuesWithWorklogsInPeriodForCurrentUser
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = new[] { new IssueAdapter(), new IssueAdapter() };
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));

            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(0))
                .Returns(Task.FromResult(new[] { this.Worklogs[1] }.AsEnumerable()));
            this.GetDependency<IJiraClient>().GetWorklogs(issues.ElementAt(1))
                .Returns(Task.FromResult(new[] { this.Worklogs[3] }.AsEnumerable()));

            var actual = (await this.Target.GetWorklogsInPeriodForUser(this.StartDate, this.EndDate)).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.IsTrue(actual[0] == this.Worklogs[1]);
        }


        [TestMethod]
        public async Task GetWorklogsInPeriodForUser_JiraReturnsNoIssues_ReturnsEmptyList()
        {
            var expectedQuery = JiraQueries.IssuesWithWorklogsInPeriodForCurrentUser
                .Replace("{startDate}", this.StartDate.AddDays(-1).ToString("yyyy-MM-dd"))
                .Replace("{endDate}", this.EndDate.ToString("yyyy-MM-dd"));
            var issues = Array.Empty<IssueAdapter>();
            this.GetDependency<IJiraClient>().GetIssuesByQuery(Arg.Is<string>(s => s == expectedQuery)).Returns(Task.FromResult((IEnumerable<IssueAdapter>)issues));

            var actual = await this.Target.GetWorklogsInPeriodForUser(this.StartDate, this.EndDate);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        #endregion

        #region GetWorklogsForIssue

        [TestMethod]
        public async Task GetWorklogsForIssue_NoFilters_ReturnsAll()
        {
            var issue = new IssueAdapter();
            this.GetDependency<IJiraClient>().GetWorklogs(issue).Returns(Task.FromResult((IEnumerable<WorklogAdapter>)this.Worklogs));

            var actual = (await this.Target.GetWorklogsForIssue(issue)).ToArray();
            Assert.AreEqual(this.Worklogs.Count, actual.Length);
            foreach (var worklog in actual) {
                Assert.IsTrue(this.Worklogs.Any(wl => wl == worklog));
            }
        }


        [TestMethod]
        public async Task GetWorklogsForIssue_WithStartDate_ReturnsNewerWorklogs()
        {
            var issue = new IssueAdapter();
            this.GetDependency<IJiraClient>().GetWorklogs(issue).Returns(Task.FromResult((IEnumerable<WorklogAdapter>)this.Worklogs));

            var actual = (await this.Target.GetWorklogsForIssue(issue, startDate: this.StartDate)).ToArray();
            Assert.AreEqual(3, actual.Length);
            Assert.IsFalse(actual.Any(wl => wl == this.Worklogs[0]));
        }


        [TestMethod]
        public async Task GetWorklogsForIssue_WithEndDate_ReturnsOlderWorklogs()
        {
            var issue = new IssueAdapter();
            this.GetDependency<IJiraClient>().GetWorklogs(issue).Returns(Task.FromResult((IEnumerable<WorklogAdapter>)this.Worklogs));

            var actual = (await this.Target.GetWorklogsForIssue(issue, endDate: this.EndDate)).ToArray();
            Assert.AreEqual(3, actual.Length);
            Assert.IsFalse(actual.Any(wl => wl == this.Worklogs[2]));
        }


        [TestMethod]
        public async Task GetWorklogsForIssue_WithUsers_ReturnsWorklogsForUser()
        {
            var issue = new IssueAdapter();
            this.GetDependency<IJiraClient>().GetWorklogs(issue).Returns(Task.FromResult((IEnumerable<WorklogAdapter>)this.Worklogs));

            var actual = (await this.Target.GetWorklogsForIssue(issue, users: new[] { this.Settings.Username })).ToArray();
            Assert.AreEqual(3, actual.Length);
            Assert.IsFalse(actual.Any(wl => wl == this.Worklogs[3]));
        }

        #endregion
    }
}