using Catharsium.CodingTools.Interface.Terminal.Models;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;

public interface IJiraIssueSelector
{
    Task<IssueAdapter> SelectIssue();
}