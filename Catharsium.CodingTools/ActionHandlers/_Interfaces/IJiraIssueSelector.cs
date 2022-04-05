using Catharsium.CodingTools.Models;
namespace Catharsium.CodingTools.ActionHandlers._Interfaces;

public interface IJiraIssueSelector
{
    Task<IssueAdapter> SelectIssue();
}