using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces
{
    public interface IIssueService
    {
        Task<IssueAdapter> GetIssueByKey(string key);

        Task<IssueAdapter> GetEpicForIssue(IssueAdapter issue);
    }
}