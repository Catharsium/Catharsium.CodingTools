using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IJiraClient
{
    Task<JiraIssue> GetIssue(string key);
    Task<IEnumerable<JiraIssue>> GetIssuesByQuery(string query);
    Task<IEnumerable<JiraWorklog>> GetWorklogs(JiraIssue issue);
    Task DeleteWorklog(string issueKey, string worklogId);
}