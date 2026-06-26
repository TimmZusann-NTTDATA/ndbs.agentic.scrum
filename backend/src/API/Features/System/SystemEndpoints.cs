namespace ndbs.shopping.API.Features.System;

public static class SystemEndpoints
{
    public static IEndpointRouteBuilder MapSystemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/system").WithTags("System");

        group.MapGet("/now", () => Results.Ok(new { timestamp = DateTimeOffset.UtcNow }))
             .WithName("GetCurrentTimestamp")
             .WithSummary("Gibt das aktuelle Datum und die Uhrzeit (UTC) zurück.");

        return app;
    }
}
