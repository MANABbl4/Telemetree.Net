# TON Solutions Telemetree library for .NET

Documentation: https://docs.ton.solutions/

## How to use in ASP.NET
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

```C#
public async Task ProcessUpdateAsync(Telegram.Bot.Types.Update update)
{
    // sending telemetree event. May be done in parallel with handling update
    try
    {
        var telemetryResult = await _telemetreeEventsHttpClient.PostEvent(update);
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