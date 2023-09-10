using RedditStats.Models;

namespace RedditStats.Services;

public interface IAnalysisService
{
    Tuple<List<string>, List<SubredditStatistic>> GetResult();
}