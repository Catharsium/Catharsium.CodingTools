using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Jira;

public class JiraActionHandler : BaseMenuActionHandler<IJiraActionHandler>
{
    public JiraActionHandler(IEnumerable<IJiraActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Jira")
    { }


    public override async Task Run()
    {
        var actionHandler = this.console.AskForItem(this.actionHandlers, "What would you like to do?");
        if (actionHandler != null) {
            await actionHandler.Run();
        }
    }
}