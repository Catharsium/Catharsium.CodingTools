using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Notify;

public class NotifyActionHandler : BaseMenuActionHandler<IGenerateActionHandler>
{
    public NotifyActionHandler(IEnumerable<IGenerateActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Notify")
    {
    }
}