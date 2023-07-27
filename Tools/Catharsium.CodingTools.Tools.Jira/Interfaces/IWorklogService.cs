using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IWorklogService
{
    Task<IEnumerable<JiraWorklog>> GetWorklogsInPeriod(DateTime startDate, DateTime endDate);
    Task<IEnumerable<JiraWorklog>> GetWorklogsInPeriodForUser(DateTime startDate, DateTime endDate);
    Task<IEnumerable<JiraWorklog>> GetWorklogsForIssueForUser(JiraIssue issue, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<JiraWorklog>> GetWorklogsForIssue(JiraIssue issue, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<string> users = null);
}