﻿using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Files.Interfaces;
using System.Text;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers
{
    public class HoursReport : BaseActionHandler, IJiraActionHandler
    {
        private readonly IWorklogService worklogService;
        private readonly IFileFactory fileFactory;

        public HoursReport(IWorklogService worklogService, IFileFactory fileFactory, IConsole console) : base(console, "Hour report")
        {
            this.worklogService = worklogService;
            this.fileFactory = fileFactory;
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
                var monthWorklogs = await this.worklogService.GetWorklogsForTeam(startDate, endDate);
                var authorWorklogs = monthWorklogs.GroupBy(wl => wl.Author);
                var csv = new StringBuilder();
                // csv.AppendLine($"\"Project\",\"Type\",\"Nummer\",\"Naam\",\"Prioriteit\",\"Labels\",\"Uren\"");
                foreach (var authorWorklog in authorWorklogs.OrderBy(i => i.Key)) {
                    var issueWorklogs = authorWorklog.GroupBy(wl => wl.Issue);

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
                    //    csv.AppendLine($"\"{issue.Key.Project}\",\"{issue.Key.Type}\",\"{issue.Key.Key}\",\"{issue.Key.Summary}\",\"{issue.Key.Priority}\",\"{labels}\",\"{timespan.TotalHours}\"");
                }
                //var file = this.fileFactory.CreateFile("D:\\\\urenrapportage.csv");
                //if (file.Exists) {
                //    file.Delete();
                //}
                //var stream = file.CreateText();
                //stream.Write(csv.ToString());
                //stream.Close();
            }

            var totalWbso = 0d;
            foreach (var userTimesheet in userTimesheets) {
                this.console.WriteLine($"{userTimesheet.Key}");
                this.console.FillBlock(0, 8);
                for (var month = 1; month <= 12; month++) {
                    var label = new DateTime(selectedYear.Value, month, 1).ToString("MMM");
                    this.console.Write($"{label}");
                    this.console.FillBlock(label.Length, 8);
                }
                this.console.WriteLine();
                for (var month = 1; month <= 12; month++) {
                    var monthlyHours = userTimesheet.Value.FirstOrDefault(xx => xx.Month == month - 1);
                    var text = monthlyHours != null ?
                        $"{monthlyHours.WbsoHours:0.0}"
                        : "";
                    this.console.Write(text);
                    this.console.FillBlock(text.Length, 8);
                }
                var wbsoSum = userTimesheet.Value.Sum(x => x.WbsoHours);
                totalWbso += wbsoSum;
                this.console.WriteLine($"{wbsoSum:0.00}");
                for (var month = 1; month <= 12; month++) {
                    var monthlyHours = userTimesheet.Value.FirstOrDefault(xx => xx.Month == month - 1);
                    var text = monthlyHours != null ?
                        $"{monthlyHours.OtherHours:0.0}"
                        : "";
                    this.console.Write(text);
                    this.console.FillBlock(text.Length, 8);
                }
                this.console.WriteLine();
            }

            this.console.WriteLine($"Totaal: {totalWbso}");
        }
    }

    public class PersonTimeSheet
    {
        public int Month { get; set; }
        public double WbsoHours { get; set; }
        public double OtherHours { get; set; }
    }
}