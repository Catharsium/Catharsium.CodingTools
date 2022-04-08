using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.Time.Extensions;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers
{
    public class TeamWeekOverviewActionHandler : BaseActionHandler, IJiraActionHandler
    {
        private readonly IWorklogService worklogService;
        private readonly IPeriodSelector periodSelector;


        public TeamWeekOverviewActionHandler(IWorklogService worklogService, IPeriodSelector periodSelector, IConsole console)
            : base(console, "Team weekoverzicht")
        {
            this.worklogService = worklogService;
            this.periodSelector = periodSelector;
        }


        public override async Task Run()
        {
            var (startDate, endDate) = this.periodSelector.SelectWorkWeek();
            var worklogs = (await this.worklogService.GetWorklogsInPeriod(startDate, endDate)).GroupBy(l => l.Author);

            this.console.WriteLine();
            this.console.WriteLine($"Overzicht voor team");
            this.console.WriteLine($"{startDate:dddd d MMMM yyyy} tot {endDate:dddd d MMMM yyyy}");
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