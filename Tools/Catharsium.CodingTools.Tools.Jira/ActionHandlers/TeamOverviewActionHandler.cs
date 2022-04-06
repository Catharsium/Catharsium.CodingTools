using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers
{
    public class TeamOverviewActionHandler : BaseActionHandler, IJiraActionHandler
    {
        private readonly IWorklogService worklogService;


        public TeamOverviewActionHandler(IWorklogService worklogService, IConsole console)
            : base(console, "Team overzicht")
        {
            this.worklogService = worklogService;
        }


        public override async Task Run()
        {
            var selectedDate = this.console.AskForDate("Which week would you like to see? (yyyy-MM-dd or empty for current week):", DateTime.Today);
            var startDate = selectedDate.GetDayOfWeek(DayOfWeek.Monday, DayOfWeek.Monday);
            var endDate = selectedDate.GetDayOfWeek(DayOfWeek.Friday, DayOfWeek.Monday);
            var worklogs = (await this.worklogService.GetWorklogsForTeam(startDate, endDate)).GroupBy(l => l.Author);

            this.console.WriteLine();
            this.console.WriteLine($"Overview for team from {startDate:dddd d MMMM yyyy} to {endDate:dddd d MMMM yyyy}");
            foreach (var group in worklogs.OrderBy(wl => wl.Key)) {
                var timeLogged = group.Select(wl => wl.TimeSpentInSeconds).Sum();
                var timespan = TimeSpan.FromSeconds(timeLogged);
                this.console.Write($"{group.Key}");
                this.console.FillBlock(group.Key.Length, 50);
                this.console.WriteLine($"{timespan.TotalHours:00}:{timespan.Minutes:00}");
            }
        }
    }
}