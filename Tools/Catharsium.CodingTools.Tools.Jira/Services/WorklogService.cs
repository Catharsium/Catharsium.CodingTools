using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Services;

public class WorklogService : IWorklogService
{
    private readonly IJiraClient jiraClient;
    private readonly JiraCodingToolsSettings settings;


    public WorklogService(IJiraClient jiraClient, JiraCodingToolsSettings settings)
    {
        this.jiraClient = jiraClient;
        this.settings = settings;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogsInPeriod(DateTime startDate, DateTime endDate)
    {
        var query = JiraQueries.IssuesWithWorklogsInPeriod
            .Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = await this.jiraClient.GetIssuesByQuery(query);

        var result = new List<WorklogAdapter>();
        foreach (var issue in issues) {
            result.AddRange(await this.GetWorklogsForIssue(issue, startDate, endDate));
        }

        return result;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogsInPeriodForUser(DateTime startDate, DateTime endDate)
    {
        var query = JiraQueries.IssuesWithWorklogsInPeriodForCurrentUser
            .Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = await this.jiraClient.GetIssuesByQuery(query);

        var result = new List<WorklogAdapter>();
        foreach (var issue in issues) {
            result.AddRange(await this.GetWorklogsForIssue(issue, startDate, endDate, new[] { this.settings.Username }));
        }

        return result;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogsForIssue(IssueAdapter issue, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<string> users = null)
    {
        var result = await this.jiraClient.GetWorklogs(issue);
        if (startDate.HasValue) {
            result = result.Where(l => l.StartDate >= startDate.Value);
        }
        if (endDate.HasValue) {
            result = result.Where(l => l.StartDate <= endDate.Value);
        }
        if (users != null) {
            result = result.Where(l => users.Contains(l.Author));
        }

        return result;
    }
}