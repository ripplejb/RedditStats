using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStats.Configs;
using RedditStats.Helpers;
using RedditStats.Models;

namespace RedditStats.Services;

public class SubredditCommentService : ISubredditCommentService
{
    private readonly HttpClient _httpClient;
    private readonly RedditConfigs _redditConfigs;
    private readonly IRateLimitHelper _rateLimitHelper;
    private readonly ILogger<SubredditCommentService> _logger;
    
    public SubredditCommentService(HttpClient httpClient, IRateLimitHelper rateLimitHelper, IOptions<RedditConfigs> redditConfigsOptions, ILogger<SubredditCommentService> logger)
    {
        _httpClient = httpClient;
        _rateLimitHelper = rateLimitHelper;
        _redditConfigs = redditConfigsOptions.Value;
        _logger = logger;

        httpClient.BaseAddress = new Uri(_redditConfigs.NoAuthBaseUrl);
    }

    public async Task<RedditResponse> Call(string uri)
    {
        var response = await _httpClient.GetAsync(uri);
        var redditResponse = new RedditResponse
        {
            RateLimit = _rateLimitHelper.GetRateLimit(response),
            SubredditData = new List<SubredditData>()
        };
        if (response.StatusCode != HttpStatusCode.OK) return redditResponse;
        var subredditData = await response.Content.ReadFromJsonAsync<List<SubredditData>>();
        if (subredditData is null)
        {
            _logger.LogError("Error receiving comments for {uri} links for {subreddit}", 
                uri, _redditConfigs.Subreddit);
            return redditResponse;
        }
        redditResponse.SubredditData.AddRange(subredditData);

        return redditResponse;
    }
}