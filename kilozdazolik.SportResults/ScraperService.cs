using HtmlAgilityPack;
using kilozdazolik.SportResutls;

public class ScraperService()
{
    private readonly string _url = @"https://www.basketball-reference.com/boxscores/";

    public string GetTitle(HtmlDocument doc)
    {
        try
        {
            var titleNode = doc.DocumentNode.SelectSingleNode("//*[@id='content']/h1");
            if (titleNode != null)
            {
                return titleNode.InnerText;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching title: " + ex.Message);
        }
        return string.Empty;
    }

    public List<TeamScore> GetBody(HtmlDocument doc)
    {
        List<TeamScore> teamScores = new List<TeamScore>();
        try
        {
            var htmlBody = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'game_summaries')]");
            if (htmlBody == null) return teamScores;
            HtmlNodeCollection childNodes = htmlBody.ChildNodes;
            foreach (var node in childNodes)
            {
                var teamsNode = node.SelectSingleNode(".//table[contains(@class, 'teams')]");
                if (teamsNode == null) continue;
                foreach (var teamNode in teamsNode.Descendants("tr"))
                {
                    var linkNode = teamNode.SelectSingleNode(".//a");
                    var scoreNode = teamNode.SelectSingleNode(".//td[contains(@class, 'right')]");
                    if (linkNode != null && scoreNode != null)
                    {
                        var teamName = linkNode.InnerText;
                        var teamScore = scoreNode.InnerText;
                        teamScores.Add(new TeamScore { TeamName = teamName, Score = teamScore });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching body: " + ex.Message);
        }
        return teamScores;

    }

    public async Task<(string Title, List<TeamScore> Matches)> ScrapeDataAsync()
    {
        List<TeamScore> teamScores = new List<TeamScore>();
        string title = "";
        HtmlWeb web = new();
        try
        {
            var doc = await web.LoadFromWebAsync(_url);
            teamScores = GetBody(doc);
            title = GetTitle(doc);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error scraping data: " + ex.Message);
        }
        return(title, teamScores);
    }
}
