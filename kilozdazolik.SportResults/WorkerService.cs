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
                DateTime goalDate = new DateTime(now.Year, now.Month, now.Day, 08, 00, 00);

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

        private string GetScrapedData(List<TeamScore> matches)
        {
            StringBuilder sb = new StringBuilder();
            int matchNumber = 1;

            for (int i = 0; i < matches.Count; i++)
            {
                if (i % 2 == 0)
                {
                    sb.AppendLine($"Match {matchNumber}");
                    sb.AppendLine("----------");
                    matchNumber++;
                }

                sb.AppendLine($"{matches[i].TeamName}: {matches[i].Score}");

                if (i % 2 != 0)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
    }
