using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Tools.IO.MorseCode.Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Notify;

public class MorseCodeNotifyActionHandler : BaseActionHandler, INotifyActionHandler
{
    private readonly IMorseCodeGenerator morseCodeGenerator;


    public MorseCodeNotifyActionHandler(IMorseCodeGenerator morseCodeGenerator, IConsole console) : base(console, "Morse code")
    {
        this.morseCodeGenerator = morseCodeGenerator;
    }


    public override async Task Run()
    {
        await Task.Run(() => {
            var text = this.console.AskForText("Enter the text:");
            this.morseCodeGenerator.SignalFor(text);
        });
    }
}