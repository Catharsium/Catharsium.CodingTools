﻿using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using System.Text;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class HoursReportActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly IWorklogService worklogService;
    private readonly ICsvFileService csvFileService;


    public HoursReportActionHandler(IWorklogService worklogService, ICsvFileService csvFileService, IConsole console)
        : base(console, "Hour report")
    {
        this.worklogService = worklogService;
        this.csvFileService = csvFileService;
    }


    public override async Task Run()
    {
        var now = DateTime.Now;
        var selectedYear = this.console.AskForInt("Enter the year:");
        if (selectedYear == null || selectedYear > now.Year) {
            selectedYear = now.Year;
        }

        var userTimesheets = new Dictionary<string, List<PersonTimeSheet>>();

        for (var month = 1; month <= 12; month++) {
            var startDate = new DateTime(selectedYear.Value, month, 1).AddDays(-1);
            var endDate = startDate.AddMonths(1);
            var monthWorklogs = await this.worklogService.GetWorklogsInPeriod(startDate, endDate);
            var authorWorklogs = monthWorklogs.GroupBy(wl => wl.Author);
            foreach (var authorWorklog in authorWorklogs.OrderBy(i => i.Key)) {
                var issueWorklogs = authorWorklog.GroupBy(wl => wl.InnerIssue);

                var wbsoIssueWorklogs = issueWorklogs.Where(iwl => iwl.Key.Labels.Any(l => l == "WBSO"));
                var wbsoSum = wbsoIssueWorklogs.SelectMany(wiwl => wiwl.ToList()).Sum(wiwl => wiwl.TimeSpentInSeconds);
                var wbsoTimeSpan = TimeSpan.FromSeconds(wbsoSum);

                var otherIssueWorklogs = issueWorklogs.Where(iwl => iwl.Key.Labels.All(l => l != "WBSO"));
                var otherSum = otherIssueWorklogs.SelectMany(wiwl => wiwl.ToList()).Sum(wiwl => wiwl.TimeSpentInSeconds);
                var otherTimeSpan = TimeSpan.FromSeconds(otherSum);

                if (userTimesheets.ContainsKey(authorWorklog.Key)) {
                    userTimesheets[authorWorklog.Key].Add(new PersonTimeSheet {
                        Month = month,
                        WbsoHours = wbsoTimeSpan.TotalHours,
                        OtherHours = otherTimeSpan.TotalHours
                    });
                }
                else {
                    userTimesheets[authorWorklog.Key] = new List<PersonTimeSheet> {new PersonTimeSheet {
                        Month = month,
                        WbsoHours = wbsoTimeSpan.TotalHours,
                        OtherHours = otherTimeSpan.TotalHours
                    } };
                }
            }
        }

        var csv = new StringBuilder();
        var totalWbso = 0d;
        csv.Append($"\"Medewerker\",\"Categorie\",");
        for (var month = 1; month <= 12; month++) {
            var label = new DateTime(selectedYear.Value, month, 1).ToString("MMM 2022");
            this.console.Write($"{label}");
            this.console.FillBlock(label.Length, 12);
            csv.Append($"\"{label}\",");
        }
        csv.AppendLine("\"Totaal\"");

        foreach (var userTimesheet in userTimesheets) {
            this.console.WriteLine($"{userTimesheet.Key}");
            this.console.FillBlock(0, 8);
            csv.Append($"\"{userTimesheet.Key}\",\"\",");
            for (var month = 1; month <= 12; month++) {
                csv.Append("\"\",");
            }
            this.console.WriteLine();
            csv.AppendLine("\"\"");

            csv.Append("\"\",\"WBSO\",");
            for (var month = 1; month <= 12; month++) {
                var monthlyHours = userTimesheet.Value.FirstOrDefault(xx => xx.Month == month);
                var text = monthlyHours != null ?
                    $"{monthlyHours.WbsoHours:0.0}"
                    : "";
                this.console.Write(text);
                this.console.FillBlock(text.Length, 12);
                csv.Append($"\"{text}\",");
            }
            var wbsoSum = userTimesheet.Value.Sum(x => x.WbsoHours);
            totalWbso += wbsoSum;
            this.console.WriteLine($"{wbsoSum:0.00}");
            csv.AppendLine($"\"{wbsoSum:0.00}\"");

            csv.Append($"\"\",\"Overig\",");
            for (var month = 1; month <= 12; month++) {
                var monthlyHours = userTimesheet.Value.FirstOrDefault(xx => xx.Month == month);
                var text = monthlyHours != null ?
                    $"{monthlyHours.OtherHours:0.0}"
                    : "";
                this.console.Write(text);
                this.console.FillBlock(text.Length, 12);
                csv.Append($"\"{text}\",");
            }
            this.console.WriteLine();
            csv.AppendLine("\"\"");
        }

        this.console.WriteLine($"Totaal: {totalWbso:0.0}");

        csv.Append($"\"Totaal\",\"\",");
        for (var month = 1; month <= 12; month++) {
            csv.Append("\"\",");
        }
        csv.AppendLine($"\"{totalWbso:0.0}\"");
        this.csvFileService.WriteToFile(csv.ToString(), $"WBSO verantwoording {selectedYear}");
    }
}