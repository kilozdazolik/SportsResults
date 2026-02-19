using Microsoft.Extensions.Hosting;
using System.Text;

namespace kilozdazolik.SportResutls
{
    public class WorkerService(EmailService emailService, ScraperService scraperService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("Worker is executing...");
                var scrapedResult = await scraperService.ScrapeDataAsync();

                DateTime now = DateTime.Now;
                DateTime goalDate = new DateTime(now.Year, now.Month, now.Day, 22, 51, 00);

                if (goalDate < now)
                {
                    goalDate = goalDate.AddDays(1);
                }

                TimeSpan time = goalDate - now;

                var scrapedData = GetScrapedData(scrapedResult.Matches);
                var title = scrapedResult.Title;
                await emailService.SendEmail("test@test.com", title, scrapedData);
                await Task.Delay(time, token);
            }
        }

        //TODO: Refactor the match count and the match number in the email body, it is not working as expected
        private string GetScrapedData(List<TeamScore> matches)
        {
            StringBuilder sb = new StringBuilder();

            byte i = 1;
            byte matchCount = 1;
            foreach (var match in matches)
            {   
                if (i % 2 != 0)
                {
                    sb.AppendLine($"Match {matchCount}");
                    sb.AppendLine("----------");
                }
                
                sb.AppendLine($"{match.TeamName}: {match.Score}");
                matchCount++;
            }
            return sb.ToString();
        }
    }
    }
