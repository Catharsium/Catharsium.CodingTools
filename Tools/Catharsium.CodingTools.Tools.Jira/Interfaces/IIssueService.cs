using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces
{
    public interface IIssueService
    {
        Task<JiraIssue> GetIssueByKey(string key);

        Task<JiraIssue> GetEpicForIssue(JiraIssue issue);
    }
}