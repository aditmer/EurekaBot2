// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.EurekaBot
{
	public class Startup
	{
		private ILoggerFactory _loggerFactory;
		private bool _isProduction = false;

		public Startup(IHostingEnvironment env)
		{
			_isProduction = env.IsProduction();

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}


		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var settings = new Settings();
			try
			{
				Configuration.GetSection("settings").Bind(settings);
				services.AddSingleton(_ => settings);
			}
			catch
			{
				var msg = @"Error reading in application settings.";
				//logger.LogError(msg);
				throw new InvalidOperationException(msg);
			}

			services.AddBot<EurekaBot>(options =>
			{
				// Creates a logger for the application to use.
				ILogger logger = _loggerFactory.CreateLogger<EurekaBot>();
				options.CredentialProvider = new SimpleCredentialProvider(Configuration["MicrosoftAppId"], Configuration["MicrosoftAppPassword"]);
				options.ChannelProvider = new ConfigurationChannelProvider(Configuration);

				// Catches any errors that occur during a conversation turn and logs them.
				options.OnTurnError = async (context, exception) =>
				{
					logger.LogError($"Exception caught : {exception}");
					await context.SendActivityAsync($"Oops - {exception.Message}");
				};
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;

			app.UseDefaultFiles()
				.UseStaticFiles()
				.UseBotFramework();
		}
	}
}
