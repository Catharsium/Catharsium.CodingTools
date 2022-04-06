using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IJiraWorklogService
{
    Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue);
}