namespace RedditStats.Models;

public class SubredditStatistic
{
    /// <summary>
    /// Subreddit or comment Id
    /// </summary>
    public string Name { get; set; }
    public int UpVotes { get; set; }
    public int NumberOfComments { get; set; }
    /// <summary>
    /// User Id 
    /// </summary>
    public string Author { get; set; }
    public DateTime CreateTime { get; set; }
}