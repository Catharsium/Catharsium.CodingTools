using Catharsium.CodingTools._Configuration;
using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.ActionHandlers.Encryption;
using Catharsium.CodingTools.ActionHandlers.Generate;
using Catharsium.CodingTools.ActionHandlers.Notify;
using Catharsium.Util.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
namespace Catharsium.CodingTools.Tests._Configuration;

[TestClass]
public class RegistrationTests
{
    [TestMethod]
    public void AddCodingTools_RegistersDependencies()
    {
        var serviceCollection = Substitute.For<IServiceCollection>();
        var config = Substitute.For<IConfiguration>();

        serviceCollection.AddCodingTools(config);
        serviceCollection.ReceivedRegistration<CodingToolsSettings>();

        serviceCollection.ReceivedRegistration<IGenerateActionHandler, GuidActionHandler>();
        serviceCollection.ReceivedRegistration<IGenerateActionHandler, RandomActionHandler>();
        serviceCollection.ReceivedRegistration<IEncryptionActionHandler, Sha256EncryptionActionHandler>();
        serviceCollection.ReceivedRegistration<INotifyActionHandler, MorseCodeNotifyActionHandler>();
    }
}