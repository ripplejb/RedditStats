namespace RedditStats.Models;

public class SubredditStatistic
{
    /// <summary>
    /// t1 = comment
    /// t3 = link
    /// </summary>
    public string Kind { get; set; } = string.Empty;
    /// <summary>
    /// comment Id
    /// </summary>
    public string Id { get; set; } = string.Empty;
    public int UpVotes { get; set; }
    /// <summary>
    /// User Id 
    /// </summary>
    public string Author { get; set; } = string.Empty;
    public string Permalink { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
}