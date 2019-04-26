// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.EurekaBot
{
	public class EurekaBot : IBot
	{
		Settings _settings;
		readonly ILogger _logger;

		public EurekaBot(ILoggerFactory loggerFactory, Settings settings)
		{
			if (loggerFactory == null)
			{
				throw new System.ArgumentNullException(nameof(loggerFactory));
			}

			_settings = settings;
			_logger = loggerFactory.CreateLogger<EurekaBot>();
			_logger.LogTrace("EurekaBot turn start.");
		}

		public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
		{
			switch (turnContext.Activity.Type)
			{
				case ActivityTypes.Message:

					//Echo back to the user what they typed
					await turnContext.SendActivityAsync($"You typed {turnContext.Activity.Text}");
					break;
			}
		}
	}
}
