﻿using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.ActionHandlers.Encryption;
using Catharsium.CodingTools.ActionHandlers.Generate;
using Catharsium.CodingTools.ActionHandlers.Notify;
using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.Util.Configuration.Extensions;
using Catharsium.Util.IO.Console._Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
namespace Catharsium.CodingTools._Configuration;

public static class Registration
{
    public static IServiceCollection AddCodingTools(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.Load<CodingToolsSettings>();
        services.AddSingleton<CodingToolsSettings, CodingToolsSettings>(provider => settings);
        if (settings != null) {
            CultureInfo.CurrentCulture = new CultureInfo(settings.LanguageCode);
        }

        services.AddConsoleIoUtilities(config);
        services.AddJiraCodingTools(config);

        services.AddScoped<IGenerateActionHandler, GuidActionHandler>();
        services.AddScoped<IGenerateActionHandler, RandomActionHandler>();
        services.AddScoped<IEncryptionActionHandler, Sha256EncryptionActionHandler>();
        services.AddScoped<INotifyActionHandler, MorseCodeNotifyActionHandler>();

        return services;
    }
}