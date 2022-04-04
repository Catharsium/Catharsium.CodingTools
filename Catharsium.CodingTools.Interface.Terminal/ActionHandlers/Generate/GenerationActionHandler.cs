using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Generate;

public class GenerationActionHandler : BaseMenuActionHandler<IGenerationActionHandler>
{
    public GenerationActionHandler(IEnumerable<IGenerationActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Generate")
    {
    }
}