using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class JiraRemoveWorklogActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraIssueSelector selectIssueSelector;
    private readonly IJiraWorklogService worklogService;


    public JiraRemoveWorklogActionHandler(
        Atlassian.Jira.Jira jira,
        IJiraIssueSelector selectIssueSelector,
        IJiraWorklogService worklogService,
        IConsole console)
        : base(console, "Delete worklog")
    {
        this.jira = jira;
        this.selectIssueSelector = selectIssueSelector;
        this.worklogService = worklogService;
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

        var worklogs = await this.worklogService.GetCurrentUserWorklogs(issue);
        if (worklogs.Any()) {
            this.console.WriteLine($"{issue.Key}\t{issue.Summary}");
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