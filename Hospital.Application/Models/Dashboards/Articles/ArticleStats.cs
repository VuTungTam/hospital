namespace Hospital.Application.Models.Dashboards.Articles
{
    public class ArticleStats
    {
        public List<ArticleStatsValue> Values { get; set; } = new();
    }

    public class ArticleStatsValue
    {
        public string Title { get; set; }

        public string ShortTitle => Title.Substring(0, Math.Min(20, Title.Length - 1)) + "...";

        public int ViewCount { get; set; }
    }
}
