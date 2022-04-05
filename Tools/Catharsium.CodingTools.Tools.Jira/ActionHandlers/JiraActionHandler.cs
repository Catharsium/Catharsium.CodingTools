using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers;

public class JiraActionHandler : BaseMenuActionHandler<IJiraActionHandler>
{
    public JiraActionHandler(IEnumerable<IJiraActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Jira")
    { }
}