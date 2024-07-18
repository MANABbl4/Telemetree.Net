# TON Solutions Telemetree library for .NET

Documentation: [https://docs.telemetree.io/](https://docs.telemetree.io/)

## How to use
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
        var telemetreeResult = await _telemetreeEventsHttpClient.SendAsync(new TelemetreeEvent()
		{
			AppName = "MyAppName",
			IsAutocapture = true,
			EventName = update.Type.ToString(),
			EventDetails = new EventDetails()
			{
				StartParameter = update.Message?.Text,
				Path = "",
				Params = new Dictionary<string, object>()
			},
			UserDetails = new EventUserDetails()
			{
				FirstName = update.Message?.From.FirstName,
				LastName = update.Message?.From.LastName,
				Username = update.Message?.From.Username,
				IsPremium = update.Message?.From.IsPremium ?? false
			},
			TelegramId = update.Message?.From?.Id.ToString(),
			Language = update.Message?.From?.LanguageCode,
			Device = "unknown",
			Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
			SessionIdentifier = "User session guid, need to implement",
		});
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