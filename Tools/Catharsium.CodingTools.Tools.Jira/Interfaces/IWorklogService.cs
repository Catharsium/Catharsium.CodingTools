using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IWorklogService
{
    Task<IEnumerable<WorklogAdapter>> GetWorklogsInPeriod(DateTime startDate, DateTime endDate);
    Task<IEnumerable<WorklogAdapter>> GetWorklogsInPeriodForUser(DateTime startDate, DateTime endDate);
    Task<IEnumerable<WorklogAdapter>> GetWorklogsForIssueForUser(IssueAdapter issue, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<WorklogAdapter>> GetWorklogsForIssue(IssueAdapter issue, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<string> users = null);
}