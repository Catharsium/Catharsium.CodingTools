using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Generate;

public class GuidActionHandler : BaseActionHandler, IGenerationActionHandler
{
    public GuidActionHandler(IConsole console)
        : base(console, "Guid")
    { }


    public override Task Run()
    {
        return Task.Run(() => {
            var guid = Guid.NewGuid().ToString();
            this.console.WriteLine("Here is your free Guid:");
            this.console.WriteLine(guid.ToString().ToUpper());
            this.console.WriteLine(guid.ToString().ToLower());
        });
    }
}