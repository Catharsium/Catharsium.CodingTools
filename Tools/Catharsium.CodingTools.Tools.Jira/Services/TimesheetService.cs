using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Services;

public class TimesheetService : ITimesheetService
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraWorklogRetriever worklogRetriever;


    public TimesheetService(Atlassian.Jira.Jira jira, IJiraWorklogRetriever worklogRetriever)
    {
        this.jira = jira;
        this.worklogRetriever = worklogRetriever;
    }


    public async Task<Dictionary<DateTime, List<WorklogAdapter>>> GetTimesheet(DateTime startDate, DateTime endDate)
    {
        var result = new Dictionary<DateTime, List<WorklogAdapter>>();
        var query = JiraQueries.LoggedDuringPeriod
            .Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = (await this.jira.Issues.GetIssuesFromJqlAsync(query)).Select(i => new IssueAdapter(i));
        foreach (var issue in issues) {
            var worklogs = await this.worklogRetriever.GetCurrentUserWorklogs(issue);
            foreach (var worklog in worklogs) {
                if (result.ContainsKey(worklog.StartDate.Date)) {
                    result[worklog.StartDate.Date].Add(worklog);
                }
                else {
                    result.Add(worklog.StartDate.Date, new List<WorklogAdapter> { worklog });
                }
            }
        }

        return result;
    }
}