using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using System.Text;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class LabelReportActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly IWorklogService worklogService;
    private readonly ICsvFileService csvFileService;


    public LabelReportActionHandler(IWorklogService worklogService, ICsvFileService csvFileService, IConsole console)
        : base(console, "Label report")
    {
        this.worklogService = worklogService;
        this.csvFileService = csvFileService;
    }


    public override async Task Run()
    {
        var now = DateTime.Now;
        var selectedYear = this.console.AskForInt("Voer het jaartal in (leeg voor huidig jaar):");
        if (selectedYear == null || selectedYear > now.Year) {
            selectedYear = now.Year;
        }
        var selectedMonth = this.console.AskForInt("Voer de maand in <1-12> (leeg voor huidige maand):");
        if (selectedMonth == null) {
            selectedMonth = now.Month;
        }
        if (selectedYear == now.Year && selectedMonth > now.Month) {
            selectedMonth = now.Month;
        }
        var startDate = new DateTime(selectedYear.Value, selectedMonth.Value, 1).AddDays(-1);
        var endDate = startDate.AddMonths(1);

        var worklogs = await this.worklogService.GetWorklogsInPeriod(startDate, endDate);
        var issueWorklogs = worklogs.GroupBy(wl => wl.Issue);
        var csv = new StringBuilder();
        csv.AppendLine($"\"Project\",\"Type\",\"Nummer\",\"Naam\",\"Prioriteit\",\"Labels\",\"Uren\"");
        foreach (var issue in issueWorklogs.OrderBy(i => i.Key.Key)) {
            this.console.WriteLine($"{issue.Key}");
            var sum = issue.Sum(wl => wl.TimeSpentInSeconds);
            var timespan = TimeSpan.FromSeconds(sum);
            var labels = string.Join(", ", issue.Key.Labels);
            this.console.WriteLine($"{timespan.TotalHours}");
            csv.AppendLine($"\"{issue.Key.Project}\",\"{issue.Key.Type}\",\"{issue.Key.Key}\",\"{issue.Key.Summary}\",\"{issue.Key.Priority}\",\"{labels}\",\"{timespan.TotalHours}\"");
        }

        this.csvFileService.WriteToFile(csv.ToString(), $"WBSO controle {selectedYear}-{selectedMonth}");
    }
}