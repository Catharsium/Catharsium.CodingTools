using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Generate;

public class GenerateActionHandler : BaseMenuActionHandler<IGenerateActionHandler>
{
    public GenerateActionHandler(IEnumerable<IGenerateActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Generate")
    {
    }
}