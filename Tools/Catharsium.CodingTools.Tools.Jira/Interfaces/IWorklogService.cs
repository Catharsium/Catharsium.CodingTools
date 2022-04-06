using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IWorklogService
{
    Task<IEnumerable<WorklogAdapter>> GetWorklogsForTeam(DateTime startDate, DateTime endDate);
    Task<IEnumerable<WorklogAdapter>> GetWorklogsForUser(DateTime startDate, DateTime endDate);
    Task<IEnumerable<WorklogAdapter>> GetWorklogs(IssueAdapter issue, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<string> users = null);
}