using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;

public interface IJiraIssueSelector
{
    Task<IssueAdapter> SelectIssue();
}