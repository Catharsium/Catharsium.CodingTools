using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class RemoveWorklogActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly IJiraIssueSelector selectIssueSelector;
    private readonly IWorklogService worklogService;


    public RemoveWorklogActionHandler(
        IJiraIssueSelector selectIssueSelector,
        IWorklogService worklogService,
        IConsole console)
        : base(console, "Uren verwijderen")
    {
        this.selectIssueSelector = selectIssueSelector;
        this.worklogService = worklogService;
    }


    public override async Task Run()
    {
        await this.Run(await this.selectIssueSelector.SelectIssue());
    }


    public async Task Run(JiraIssue issue)
    {
        if (issue == null) {
            this.console.WriteLine("Geen geldig issue geselecteerd.");
            return;
        }

        var worklogs = (await this.worklogService.GetWorklogsForIssueForUser(issue));
        if (worklogs.Any()) {
            this.console.WriteLine($"{issue.Key}\t{issue.Summary}");
            var selectedWorklogItem = this.console.AskForItem(worklogs);
            if (selectedWorklogItem != null) {
                await issue.DeleteWorklogAsync(selectedWorklogItem);
                this.console.WriteLine($"Worklog '{selectedWorklogItem}' was verwijderd");
            }
        }
        else {
            this.console.WriteLine("U heeft geen uren gelogd op dit issue.");
        }
    }
}