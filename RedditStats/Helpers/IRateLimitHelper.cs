using RedditStats.Models;

namespace RedditStats.Helpers;

public interface IRateLimitHelper
{
    RateLimit GetRateLimit(HttpResponseMessage responseMessage);
}