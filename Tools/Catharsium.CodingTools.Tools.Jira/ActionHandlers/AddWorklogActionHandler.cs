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
        : base(console, "Add worklog")
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
            this.console.WriteLine("No issue was selected.");
            return;
        }

        var worklogs = await this.worklogService.GetWorklogs(issue);
        foreach (var worklog in worklogs) {
            this.console.WriteLine(worklog.ToString());
        }
        var timespent = this.console.AskForText("Time spent (in Jira notation)");
        var startDate = this.console.AskForDate("Date for the worklog (yyyy-MM-dd)", DateTime.Today);
        await issue.AddWorklogAsync(new Worklog(timespent, startDate));
    }
}