using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public class WorklogService : IWorklogService
{
    private readonly IJiraClient jiraClient;
    private readonly JiraCodingToolsSettings settings;


    public WorklogService(IJiraClient jiraClient, JiraCodingToolsSettings settings)
    {
        this.jiraClient = jiraClient;
        this.settings = settings;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogsForTeam(DateTime startDate, DateTime endDate)
    {
        var query = JiraQueries.IssuesForUsersWithWorklogsInPeriod
            .Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = (await this.jiraClient.GetIssuesByQuery(query));

        var result = new List<WorklogAdapter>();
        foreach (var issue in issues) {
            result.AddRange(await this.GetWorklogs(issue, startDate, endDate, this.settings.TeamMembers));
        }

        return result;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogsForUser(DateTime startDate, DateTime endDate)
    {
        var query = JiraQueries.IssuesForCurrentUserWithWorklogsInPeriod
            .Replace("{startDate}", startDate.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
        var issues = (await this.jiraClient.GetIssuesByQuery(query));

        var result = new List<WorklogAdapter>();
        foreach (var issue in issues) {
            result.AddRange(await this.GetWorklogs(issue, startDate, endDate, new[] { this.settings.Username }));
        }

        return result;
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogs(IssueAdapter issue, DateTime? startDate, DateTime? endDate, IEnumerable<string> users = null)
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