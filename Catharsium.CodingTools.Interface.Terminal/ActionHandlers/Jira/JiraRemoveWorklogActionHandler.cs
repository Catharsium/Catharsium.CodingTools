using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;

namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira
{
    public class JiraRemoveWorklogActionHandler : BaseActionHandler, IJiraActionHandler
    {
        private readonly CodingToolsTerminalSettings settings;

        public Atlassian.Jira.Jira Jira { get; }


        public JiraRemoveWorklogActionHandler(IConsole console, CodingToolsTerminalSettings settings)
            : base(console, "Delete worklog")
        {
            this.settings = settings;
            this.Jira = Atlassian.Jira.Jira.CreateRestClient("https://tickets.unifiedpost.com", this.settings.Username, this.settings.Password);
        }


        public async Task Run(IssueAdapter issue)
        {
            if (issue == null) {
                this.console.WriteLine("Issue was not found");
                return;
            }

            var worklogs = (await this.Jira.Issues.GetWorklogsAsync(issue.InternalIssue.Key.Value)).Where(l => l.Author == this.settings.Username).Select(l => new WorklogAdapter(l));
            if (worklogs.Any()) {
                this.console.WriteLine($"{issue.InternalIssue.JiraIdentifier}\t{issue.InternalIssue.Key}");
                var selectedWorklogItem = this.console.AskForItem(worklogs);
                if (selectedWorklogItem != null) {
                    await this.Jira.Issues.DeleteWorklogAsync(issue.InternalIssue.Key.Value, selectedWorklogItem.InternalWorklog.Id);
                    this.console.WriteLine($"Worklog '{selectedWorklogItem}' was deleted");
                }
            }
            else {
                this.console.WriteLine("You have no work logged on this ticket.");
            }
        }


        public override async Task Run()
        {
            var issues = await this.Jira.Issues.GetIssuesFromJqlAsync("project = IDN AND sprint in openSprints() AND (issuetype = Story OR issuetype = Bug OR issuetype = Improvement)");
            var issueAdapters = issues.Select(i => new IssueAdapter(i));
            var selectedIssue = this.console.AskForItem(issueAdapters, "Which issue?");
            if (selectedIssue == null) {
                var selectedIssueKey = this.console.AskForText("Enter the issue key");
                var manualIssue = await this.Jira.Issues.GetIssueAsync("IDN-" + selectedIssueKey);
                if (manualIssue == null) {
                    this.console.WriteLine($"Issue '{"IDN-" + selectedIssueKey}' was not found");
                    return;
                }
                selectedIssue = new IssueAdapter(manualIssue);
            }
            await this.Run(selectedIssue);
        }
    }
}