using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira;

internal class JiraWorklogOverviewActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraWorklogRetriever worklogRetriever;


    public JiraWorklogOverviewActionHandler(Atlassian.Jira.Jira jira, IJiraWorklogRetriever worklogRetriever, IConsole console)
        : base(console, "Worklog overview")
    {
        this.jira = jira;
        this.worklogRetriever = worklogRetriever;
    }


    public override async Task Run()
    {
        var selectedDate = this.console.AskForDate("Which week would you like to see? (yyyy-MM-dd or empty for current week):", DateTime.Today);
        var startDate = selectedDate.GetDayOfWeek(DayOfWeek.Monday, DayOfWeek.Monday);
        var endDate = selectedDate.GetDayOfWeek(DayOfWeek.Friday, DayOfWeek.Monday);
        var timesheet = new Dictionary<DateTime, List<WorklogAdapter>>();

        var query = JiraQueries.LoggedDuringPeriod.Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd")).Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = (await this.jira.Issues.GetIssuesFromJqlAsync(query)).Select(i => new IssueAdapter(i));
        foreach (var issue in issues) {
            var worklogs = await this.worklogRetriever.GetCurrentUserWorklogs(issue);
            foreach (var worklog in worklogs) {
                if (timesheet.ContainsKey(worklog.InternalWorklog.StartDate.Value.Date)) {
                    timesheet[worklog.InternalWorklog.StartDate.Value.Date].Add(worklog);
                }
                else {
                    timesheet.Add(worklog.InternalWorklog.StartDate.Value.Date, new List<WorklogAdapter> { worklog });
                }
            }
        }

        while (startDate <= endDate) {
            if (timesheet.ContainsKey(startDate)) {
                var totalSecondsLogged = timesheet[startDate].Select(l => l.InternalWorklog.TimeSpentInSeconds).Sum();
                var totalTimeLogged = TimeSpan.FromSeconds(totalSecondsLogged);
                if (totalTimeLogged.TotalHours < 8) {
                    this.console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else {
                    this.console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                var formattedStartDate = startDate.ToString("dddd d MMMM yyyy");
                if(formattedStartDate.Length < 24) {
                    formattedStartDate += "\t";
                }
                this.console.WriteLine($"{formattedStartDate}\t{totalTimeLogged.Hours} hours {totalTimeLogged.Minutes} minutes logged");
                this.console.ResetColor();
                foreach (var log in timesheet[startDate]) {
                    this.console.WriteLine($"\t{log.ToReferenceString()}");
                }
            }
            else {
                this.console.ForegroundColor = ConsoleColor.DarkRed;
                this.console.WriteLine($"{startDate:dddd d MMMM yyyy}");
                this.console.ResetColor();
                this.console.WriteLine("\tNo worklogs");
            }
            startDate = startDate.AddDays(1);
        }
    }
}