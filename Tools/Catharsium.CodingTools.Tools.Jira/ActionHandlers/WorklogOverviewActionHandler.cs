using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class WorklogOverviewActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly ITimesheetService timesheetService;


    public WorklogOverviewActionHandler(ITimesheetService timesheetService, IConsole console)
        : base(console, "Worklog overview")
    {
        this.timesheetService = timesheetService;
    }


    public override async Task Run()
    {
        var selectedDate = this.console.AskForDate("Which week would you like to see? (yyyy-MM-dd or empty for current week):", DateTime.Today);
        var startDate = selectedDate.GetDayOfWeek(DayOfWeek.Monday, DayOfWeek.Monday);
        var endDate = selectedDate.GetDayOfWeek(DayOfWeek.Friday, DayOfWeek.Monday);
        var timesheet = await this.timesheetService.GetTimesheet(startDate, endDate);

        while (startDate <= endDate) {
            if (timesheet.ContainsKey(startDate)) {
                var totalSecondsLogged = timesheet[startDate].Select(l => l.TimeSpentInSeconds).Sum();
                var totalTimeLogged = TimeSpan.FromSeconds(totalSecondsLogged);
                if (totalTimeLogged.TotalHours < 8) {
                    this.console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else {
                    this.console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                var formattedStartDate = startDate.ToString("dddd d MMMM yyyy");
                if (formattedStartDate.Length < 24) {
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