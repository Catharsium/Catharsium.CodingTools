using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using System.Security.Cryptography;
using System.Text;
namespace Catharsium.CodingTools.ActionHandlers.Encryption;

public class EncryptionActionHandler : BaseMenuActionHandler<IEncryptionActionHandler>
{
    public EncryptionActionHandler(IEnumerable<IEncryptionActionHandler> actionHandlers, IConsole console)
        : base(actionHandlers, console, "Encrypt")
    { }


    public override Task Run()
    {
        return Task.Run(() => {
            var key = this.console.AskForText("Enter the encryption key");
            var message = this.console.AskForText("Enter the message");
            using var HMACSHA256 = new HMACSHA256(new UTF8Encoding().GetBytes(key));
            var hashedMessage = HMACSHA256.ComputeHash(new UTF8Encoding().GetBytes(message));
            var hashedMessageString = BitConverter.ToString(hashedMessage).Replace("-", "").ToLower();
            this.console.WriteLine();
            this.console.WriteLine($"Message encoded using {typeof(HMACSHA256).Name}:");
            this.console.WriteLine(hashedMessageString);
        });
    }
}