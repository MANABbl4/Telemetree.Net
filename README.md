# TON Solutions Telemetree library for .NET

Documentation: [https://docs.telemetree.io/](https://docs.telemetree.io/)

## How to use in ASP.NET
### Add to ASP.NET
```C#
builder.Services
    .AddHttpClient("telemetree_client")
    .AddTypedClient<TelemetreeEventsHttpClient>((httpClient, sp) =>
    {
        var configTask = TelemetreeHelper.GetTelemetreeConfig("YOUR_API_KEY", "YOUR_PROJECT_ID");
        configTask.Wait();

        return new TelemetreeEventsHttpClient(configTask.Result, "YOUR_API_KEY", "YOUR_PROJECT_ID", httpClient);
    });
```

### Or just create
```C#
    var config = await TelemetreeHelper.GetTelemetreeConfig("YOUR_API_KEY", "YOUR_PROJECT_ID");
    var telemetreeClient = new TelemetreeEventsHttpClient(configTask.Result, "YOUR_API_KEY", "YOUR_PROJECT_ID");
```

### Send Telegram bot update data
```C#
public async Task ProcessUpdateAsync(Telegram.Bot.Types.Update update)
{
    // sending telemetree event. Could be done in parallel with handling update
    try
    {
        var telemetryResult = await _telemetreeEventsHttpClient.SendAsync(update);
    }
    catch (Exception ex)
    {
        await HandlerErrorAsync(ex);
    }
	
    // your update processing code
    var handler = update.Type switch
    {
        UpdateType.Message => BotOnMessageRecieved(update.Message!),
        UpdateType.CallbackQuery => BotOnCallBackQueryRecieved(update.CallbackQuery!),
        UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery!),
        _ => UnknownUpdateTypeHandler(update)
    };

    try
    {
        await handler;
    }
    catch (Exception ex)
    {
        await HandlerErrorAsync(ex);
    }
}
```

### Send some event data
```C#
    var telemetryResult = await _telemetreeEventsHttpClient.SendAsync(new TelemetreeEvent()
	{
		AppName = "YOUR_APP_NAME",
		EventName = "MainButtonPressed",
		UserDetails = new EventUserDetails()
		{
			Username = "User name",
			FirstName = "First name"
		},
		EventDetails = new EventDetails()
		{
			Path = "MainButton location",
			Params = new Dictionary<string, object>() // your MainButton additional params
		},
		TelegramId = "Telegram user Id",
		Language = "Telegram user Lang",
		Device = "Telegram user Device",
		Referrer = "N/A",
		ReferrerType = "0",
		Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
		IsAutocapture = false,
		Wallet = null,
		SessionIdentifier = "User session guid, need to implement"
	});
```