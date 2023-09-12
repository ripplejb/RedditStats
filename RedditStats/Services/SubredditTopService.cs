using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStats.Configs;
using RedditStats.Helpers;
using RedditStats.Models;

namespace RedditStats.Services;

public class SubredditTopService : ISubredditTopService
{
    private readonly HttpClient _httpClient;
    private readonly RedditConfigs _redditConfigs;
    private readonly IRateLimitHelper _rateLimitHelper;
    private readonly ILogger<SubredditTopService> _logger;
    
    public SubredditTopService(HttpClient httpClient, IRateLimitHelper rateLimitHelper, IOptions<RedditConfigs> redditConfigsOptions, ILogger<SubredditTopService> logger)
    {
        _httpClient = httpClient;
        _rateLimitHelper = rateLimitHelper;
        _redditConfigs = redditConfigsOptions.Value;
        _logger = logger;

        httpClient.BaseAddress = new Uri(_redditConfigs.NoAuthBaseUrl);
    }


    public async Task<RedditResponse> Call(int topCount, string period)
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", new []{"RedditStats",
            Environment.OSVersion.Platform.ToString(), Environment.OSVersion.VersionString});
        var response = await _httpClient.GetAsync($"/r/{_redditConfigs.Subreddit}/top.json?limit={topCount}&t={period}");
        var redditResponse = new RedditResponse
        {
            RateLimit = _rateLimitHelper.GetRateLimit(response),
            SubredditData = new List<SubredditData>()
        };
        if (response.StatusCode != HttpStatusCode.OK) return redditResponse;
        var subredditData = await response.Content.ReadFromJsonAsync<SubredditData>();
        if (subredditData is null)
        {
            _logger.LogError("Error receiving top {topCount} links for {subreddit}", 
                topCount, _redditConfigs.Subreddit);
            return redditResponse;
        }
        redditResponse.SubredditData.Add(subredditData);

        return redditResponse;
    }
}