using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;

public interface IJiraWorklogRetriever
{
    Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue);
}