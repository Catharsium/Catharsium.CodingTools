using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
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
        var startDate = this.console.AskForDate(DateTime.Today, "Datum <yyyy-MM-dd> (leeg voor vandaag)");
        var timesheet = await this.worklogService.GetWorklogsInPeriodForUser(startDate, startDate);
        var alreadyLoggedTime = TimeSpan.FromSeconds(timesheet.Select(wl => wl.TimeSpentInSeconds).Sum());
        var remainingTimeToLog = TimeSpan.FromHours(8) - alreadyLoggedTime;

        var issue = await this.jiraSelectIssueSelector.SelectIssue();
        var worklogs = await this.worklogService.GetWorklogsForIssueForUser(issue);
        foreach (var worklog in worklogs.Where(wl => wl.StartDate >= startDate.AddDays(-7))) {
            this.console.WriteLine(worklog.ToString());
        }

        this.console.Write("Je hebt reeds ");
        this.console.ForegroundColor = remainingTimeToLog.TotalMinutes > 0
            ? ConsoleColor.DarkRed
            : ConsoleColor.DarkGreen;
        this.console.Write($"{alreadyLoggedTime.TotalHours}");
        this.console.ResetColor();
        this.console.WriteLine($" uur op {startDate:dddd d MMMM yyyy} gelogd.");

        var timespent = this.console.AskForText("Bestede uren <#d #h #m> (leeg voor resterende tijd)");
        if (string.IsNullOrWhiteSpace(timespent) && remainingTimeToLog.TotalMinutes > 0) {
            timespent = $"{remainingTimeToLog.TotalMinutes}m";
        }
        if (string.IsNullOrWhiteSpace(timespent)) {
            this.console.WriteLine("Er is geen nieuwe tijd gelogd.");
            return;
        }

        var newWorklog = new Worklog(timespent, startDate);
        await issue.AddWorklogAsync(newWorklog);
        this.console.WriteLine($"{timespent} was gelogd op {issue.Key} ({startDate:dddd d MMMM yyyy})");
    }
}