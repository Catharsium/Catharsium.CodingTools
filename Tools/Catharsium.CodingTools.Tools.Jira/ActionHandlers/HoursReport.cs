using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Files.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var startDate = new DateTime(2021, 12, 31);
            var endDate = startDate.AddMonths(1);
            var x = await this.worklogService.GetWorklogsForTeam(startDate, endDate);
            var authorWorklogs = x.GroupBy(wl => wl.Author);
            var csv = new StringBuilder();
           // csv.AppendLine($"\"Project\",\"Type\",\"Nummer\",\"Naam\",\"Prioriteit\",\"Labels\",\"Uren\"");
            foreach (var authorWorklog in authorWorklogs.OrderBy(i => i.Key)) {
                this.console.WriteLine($"{authorWorklog.Key}");
                var issueWorklogs = authorWorklog.GroupBy(wl => wl.Issue);

                var wbsoIssueWorklogs = issueWorklogs.Where(iwl => iwl.Key.Labels.Any(l => l == "WBSO"));
                var wbsoSum = wbsoIssueWorklogs.SelectMany(wiwl => wiwl.ToList()).Sum(wiwl => wiwl.TimeSpentInSeconds);
                var wbsoTimeSpan = TimeSpan.FromSeconds(wbsoSum);

                var otherIssueWorklogs = issueWorklogs.Where(iwl => iwl.Key.Labels.All(l => l != "WBSO"));
                var otherSum = otherIssueWorklogs.SelectMany(wiwl => wiwl.ToList()).Sum(wiwl => wiwl.TimeSpentInSeconds);
                var otherTimeSpan = TimeSpan.FromSeconds(otherSum);

                this.console.WriteLine($"WBSO: {wbsoTimeSpan.TotalHours}");
                this.console.WriteLine($"Other: {otherTimeSpan.TotalHours}");
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
    }
}
