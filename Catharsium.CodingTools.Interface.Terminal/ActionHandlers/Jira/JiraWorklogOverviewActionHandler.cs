using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira;

internal class JiraWorklogOverviewActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly CodingToolsTerminalSettings settings;

    public JiraWorklogOverviewActionHandler(IConsole console, CodingToolsTerminalSettings settings)
        : base(console, "Worklog overview")
    {
        this.settings = settings;
    }


    public override async Task Run()
    {
        var jira = Atlassian.Jira.Jira.CreateRestClient("https://tickets.unifiedpost.com", this.settings.Username, this.settings.Password);
        var issues = await jira.Issues.GetIssuesFromJqlAsync("project = IDN AND sprint in openSprints() AND (issuetype = Story OR issuetype = Bug OR issuetype = Improvement)");
        foreach (var issue in issues) {
            var log = (await jira.Issues.GetWorklogsAsync(issue.Key.Value)).Where(l => l.Author == this.settings.Username);
            if (log.Any()) {
                var dates = log.Select(l => l.StartDate.Value).Distinct().OrderBy(d => d);
                this.console.WriteLine($"{issue.JiraIdentifier}\t{issue.Key}");
                foreach (var date in dates) {
                    var timeSpend = log.Where(l => l.StartDate.Value == date).Select(l => l.TimeSpentInSeconds).Sum();
                    this.console.WriteLine($"{date:yyyy-MM-dd}\t{this.ToTime(timeSpend)}");
                }
            }
        }
    }


    private string ToTime(long timeSpend)
    {
        var hours = timeSpend / 60 / 60;
        var minutes = (timeSpend / 60) % 60;
        return $"{hours} hours, {minutes} minutes";
    }
}