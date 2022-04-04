using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira;

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