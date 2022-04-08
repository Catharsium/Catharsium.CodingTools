using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class PersonalWeekOverviewActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly ITimesheetService timesheetService;
    private readonly IPeriodSelector periodSelector;


    public PersonalWeekOverviewActionHandler(ITimesheetService timesheetService, IPeriodSelector periodSelector, IConsole console)
        : base(console, "Persoonlijk weekoverzicht")
    {
        this.timesheetService = timesheetService;
        this.periodSelector = periodSelector;
    }


    public override async Task Run()
    {
        var (startDate, endDate) = this.periodSelector.SelectWorkWeek();
        var timesheet = await this.timesheetService.GetTimesheet(startDate, endDate);

        this.console.WriteLine();
        this.console.WriteLine($"Week overzicht");
        this.console.WriteLine($"{startDate:dddd d MMMM yyyy} tot {endDate:dddd d MMMM yyyy}");

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
                this.console.WriteLine($"{formattedStartDate}\t{totalTimeLogged.Hours}:{totalTimeLogged.Minutes} gelogd");
                this.console.ResetColor();
                foreach (var log in timesheet[startDate]) {
                    this.console.WriteLine($"\t{log.ToReferenceString()}");
                }
            }
            else {
                this.console.ForegroundColor = ConsoleColor.DarkRed;
                this.console.WriteLine($"{startDate:dddd d MMMM yyyy}");
                this.console.ResetColor();
                this.console.WriteLine("\tGeen tijd gelogd");
            }
            startDate = startDate.AddDays(1);
        }
    }
}