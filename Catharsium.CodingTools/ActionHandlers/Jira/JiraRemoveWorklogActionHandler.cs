using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Jira;

public class JiraRemoveWorklogActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraIssueSelector selectIssueSelector;
    private readonly IJiraWorklogRetriever worklogRetriever;

    public JiraRemoveWorklogActionHandler(Atlassian.Jira.Jira jira, IJiraIssueSelector selectIssueSelector, IJiraWorklogRetriever WorklogRetriever, IConsole console)
        : base(console, "Delete worklog")
    {
        this.jira = jira;
        this.selectIssueSelector = selectIssueSelector;
        this.worklogRetriever = WorklogRetriever;
    }


    public override async Task Run()
    {
        await this.Run(await this.selectIssueSelector.SelectIssue());
    }


    public async Task Run(IssueAdapter issue)
    {
        if (issue == null) {
            this.console.WriteLine("No issue was selected.");
            return;
        }

        var worklogs = await this.worklogRetriever.GetCurrentUserWorklogs(issue);
        if (worklogs.Any()) {
            this.console.WriteLine($"{issue.JiraIdentifier}\t{issue.Key}");
            var selectedWorklogItem = this.console.AskForItem(worklogs);
            if (selectedWorklogItem != null) {
                await this.jira.Issues.DeleteWorklogAsync(issue.Key, selectedWorklogItem.Id);
                this.console.WriteLine($"Worklog '{selectedWorklogItem}' was deleted");
            }
        }
        else {
            this.console.WriteLine("You have no work logged on this ticket.");
        }
    }
}