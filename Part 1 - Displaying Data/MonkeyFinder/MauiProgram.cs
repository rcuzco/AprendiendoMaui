﻿using Microsoft.Extensions.DependencyInjection;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IDisplayMessageService, DisplayMessageService>();
        builder.Services.AddSingleton<IMonkeyService, MonkeyService>();
		
		builder.Services.AddSingleton<MonkeysViewModel>();
        builder.Services.AddTransient<MonkeyDetailsViewModel>();
        
		builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailsPage>();
        return builder.Build();
	}
}
