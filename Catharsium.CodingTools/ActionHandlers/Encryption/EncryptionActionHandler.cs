using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Encryption;

public class EncryptionActionHandler : BaseMenuActionHandler<IEncryptionActionHandler>
{
    public EncryptionActionHandler(IEnumerable<IEncryptionActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Encrypt")
    { }
}