using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IJiraClient
{
    Task<IssueAdapter> GetIssue(string key);
    Task<IEnumerable<IssueAdapter>> GetIssuesByQuery(string query);
    Task<IEnumerable<WorklogAdapter>> GetWorklogs(IssueAdapter issue);
    Task DeleteWorklog(string issueKey, string worklogId);
}