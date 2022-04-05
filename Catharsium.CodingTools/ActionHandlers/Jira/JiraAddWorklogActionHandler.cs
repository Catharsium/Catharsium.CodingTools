using Atlassian.Jira;
using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;

namespace Catharsium.CodingTools.ActionHandlers.Jira
{
    public class JiraAddWorklogActionHandler : BaseActionHandler, IJiraActionHandler
    {
        private readonly Atlassian.Jira.Jira jira;
        private readonly IJiraIssueSelector jiraSelectIssueSelector;


        public JiraAddWorklogActionHandler(Atlassian.Jira.Jira jira, IJiraIssueSelector jiraSelectIssueSelector, IConsole console)
            : base(console, "Add worklog")
        {
            this.jira = jira;
            this.jiraSelectIssueSelector = jiraSelectIssueSelector;
        }


        public override async Task Run()
        {
            await this.Run(await this.jiraSelectIssueSelector.SelectIssue());
        }


        public async Task Run(IssueAdapter issue)
        {
            if (issue == null) {
                this.console.WriteLine("No issue was selected.");
                return;
            }

            var worklogs = (await this.jira.Issues.GetWorklogsAsync(issue.Key)).Select(l => new WorklogAdapter(l, issue));
            foreach (var worklog in worklogs) {
                this.console.WriteLine(worklog.ToString());
            }
            var timespent = this.console.AskForText("Time spent (in Jira notation)");
            var startDate = this.console.AskForDate("Date for the worklog (yyyy-MM-dd)", DateTime.Today);
            await issue.AddWorklogAsync(new Worklog(timespent, startDate));
        }
    }
}