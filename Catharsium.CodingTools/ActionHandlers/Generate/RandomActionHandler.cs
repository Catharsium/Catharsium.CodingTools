using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.ActionHandlers.Generate;

public class RandomActionHandler : BaseActionHandler, IGenerateActionHandler
{
    public RandomActionHandler(IConsole console) : base(console, "Random")
    {
    }


    public override async Task Run()
    {
        await Task.Run(() => {
            var random = new Random();
            int minValue;
            int maxValue;
            while(true) {
                this.console.WriteLine("Generate a random number between inclusive boundaries.");
                this.console.WriteLine("To quit, make the boundaries equal to each other.");
                maxValue = this.console.AskForInt(10, "Enter the maximum value (default 10)");
                minValue = this.console.AskForInt(1, "Enter the minimum value (default 1)");
                if(minValue == maxValue) {
                    break;
                }

                if(maxValue < minValue) {
                    (maxValue, minValue) = (minValue, maxValue);
                }

                bool next;
                do {
                    this.console.WriteLine("Your next random number is");
                    this.console.ForegroundColor = ConsoleColor.Red;
                    this.console.WriteLine(random.Next(minValue, maxValue + 1).ToString());
                    this.console.ResetColor();

                    var nextInput = this.console.AskForText("Continu? [y/n] (empty is yes)");
                    next = string.IsNullOrWhiteSpace(nextInput)
                        || nextInput.Equals("y", StringComparison.InvariantCultureIgnoreCase)
                        || nextInput.Equals("yes", StringComparison.InvariantCultureIgnoreCase);

                } while(next);
            };
        });
    }
}