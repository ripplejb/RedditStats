using RedditStats.Models;

namespace RedditStats.Helpers;

public class RateLimitHelper : IRateLimitHelper
{
    public RateLimit GetRateLimit(HttpResponseMessage responseMessage)
    {
        var remaining = 0;
        var reset = 0;
        if (responseMessage.Headers.TryGetValues("x-ratelimit-remaining", out var values))
        {
            int.TryParse(values.First(), out remaining);
        }
        if (responseMessage.Headers.TryGetValues("x-ratelimit-reset", out values))
        {
            int.TryParse(values.First(), out reset);
        }

        return new RateLimit
        {
            RemainingCalls = remaining,
            ResetSeconds = reset
        };
    }
}