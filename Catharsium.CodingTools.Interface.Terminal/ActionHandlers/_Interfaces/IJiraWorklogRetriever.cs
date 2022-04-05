using Catharsium.CodingTools.Interface.Terminal.Models;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;

public interface IJiraWorklogRetriever
{
    Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue);
}