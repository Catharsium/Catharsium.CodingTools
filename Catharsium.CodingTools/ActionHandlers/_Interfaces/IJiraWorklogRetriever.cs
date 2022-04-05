using Catharsium.CodingTools.Models;

namespace Catharsium.CodingTools.ActionHandlers._Interfaces;

public interface IJiraWorklogRetriever
{
    Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue);
}