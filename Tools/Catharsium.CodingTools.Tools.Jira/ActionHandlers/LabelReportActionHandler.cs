using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using System.Text;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class LabelReportActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly IWorklogService worklogService;
    private readonly IIssueService issueService;
    private readonly ICsvFileService csvFileService;


    public LabelReportActionHandler(IWorklogService worklogService, IIssueService issueService, ICsvFileService csvFileService, IConsole console)
        : base(console, "Label report")
    {
        this.worklogService = worklogService;
        this.issueService = issueService;
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
        selectedMonth ??= now.Month;
        if (selectedYear == now.Year && selectedMonth > now.Month) {
            selectedMonth = now.Month;
        }

        var startDate = new DateTime(selectedYear.Value, selectedMonth.Value, 1).AddDays(-1);
        var endDate = startDate.AddMonths(1);

        var worklogs = await this.worklogService.GetWorklogsInPeriod(startDate, endDate);
        var issueWorklogs = worklogs.GroupBy(wl => wl.InnerIssue);
        var csv = new StringBuilder();

        csv.AppendLine($"\"Project\",\"Epic\",\"Type\",\"Nummer\",\"Naam\",\"Prioriteit\",\"Labels\",\"Uren\"");
        foreach (var issue in issueWorklogs.OrderBy(i => i.Key.Key)) {
            var epicName = "";
            var epicIssue = await this.issueService.GetEpicForIssue(issue.Key);
            if (epicIssue != null && epicIssue.CustomFields.ContainsKey(CustomFields.EpicName)) {
                epicName = epicIssue.CustomFields[CustomFields.EpicName]?[0];
            }

            this.console.WriteLine($"{issue.Key}");
            var sum = issue.Sum(wl => wl.TimeSpentInSeconds);
            var timespan = TimeSpan.FromSeconds(sum);
            var labels = string.Join(", ", issue.Key.Labels);
            this.console.WriteLine($"{timespan.TotalHours}");
            csv.AppendLine($"\"{issue.Key.Project}\",\"{epicName}\",\"{issue.Key.Type}\",\"{issue.Key.Key}\",\"{issue.Key.Summary}\",\"{issue.Key.Priority}\",\"{labels}\",\"{timespan.TotalHours}\"");
        }

        this.csvFileService.WriteToFile(csv.ToString(), $"WBSO controle {selectedYear}-{selectedMonth}");
    }
}