using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class AddWorklogActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly IJiraIssueSelector jiraSelectIssueSelector;
    private readonly IWorklogService worklogService;


    public AddWorklogActionHandler(
        IJiraIssueSelector jiraSelectIssueSelector,
        IWorklogService worklogService,
        IConsole console)
        : base(console, "Uren toevoegen")
    {
        this.jiraSelectIssueSelector = jiraSelectIssueSelector;
        this.worklogService = worklogService;
    }


    public override async Task Run()
    {
        await this.Run(await this.jiraSelectIssueSelector.SelectIssue());
    }


    public async Task Run(IssueAdapter issue)
    {
        if (issue == null) {
            this.console.WriteLine("Geen geldig issue geselecteerd.");
            return;
        }

        var worklogs = await this.worklogService.GetWorklogsForIssue(issue);
        foreach (var worklog in worklogs) {
            this.console.WriteLine(worklog.ToString());
        }
        var timespent = this.console.AskForText("Bestede uren <#d #h #m>");
        var startDate = this.console.AskForDate("Datum <yyyy-MM-dd> (leeg voor vandaag)", DateTime.Today);
        await issue.AddWorklogAsync(new Worklog(timespent, startDate));
    }
}