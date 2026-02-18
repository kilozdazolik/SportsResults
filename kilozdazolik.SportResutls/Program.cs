using HtmlAgilityPack;

var html = @"https://www.basketball-reference.com/boxscores/";

HtmlWeb web = new HtmlWeb();

var htmlDoc = web.Load(html);

var title = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"content\"]/h1").First().InnerText;

Console.WriteLine(title);

// game_summaries
var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]");

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
            Console.WriteLine(teamName + ": " + teamScore);
        }
    }

} 



Console.ReadKey();