using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;

public class PeriodSelector : IPeriodSelector
{
    private readonly IConsole console;


    public PeriodSelector(IConsole console)
    {
        this.console = console;
    }


    public (DateTime startDate, DateTime endDate) SelectWorkWeek()
    {
        var selectedDate = this.console.AskForDate(DateTime.Today, "Kies een week o.b.v. een datum <yyyy-MM-dd> (leeg voor vandaag):");
        var startDate = selectedDate.GetDayOfWeek(DayOfWeek.Monday, DayOfWeek.Monday);
        var endDate = selectedDate.GetDayOfWeek(DayOfWeek.Friday, DayOfWeek.Monday);
        return (startDate, endDate);
    }
}