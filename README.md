# Sport Results Notifier

A .NET background worker service that scrapes daily NBA scores from Basketball Reference and delivers them via email at a scheduled time every day.

---

## Overview

Sport Results Notifier is a lightweight, headless .NET application built as a hosted background service. It automatically scrapes basketball game results and formats them into a clean email digest sent once per day at a configured time.

---

## Features

- Scrapes live box score data from [basketball-reference.com](https://www.basketball-reference.com/boxscores/)
- Formats match results into a readable email body
- Sends emails via SMTP using configurable settings
- Runs as a .NET `BackgroundService` with daily scheduling via `Task.Delay`

---

## Project Structure

| File | Responsibility |
|---|---|
| `ScraperService.cs` | Scrapes team names and scores using HtmlAgilityPack |
| `EmailService.cs` | Sends formatted results via SMTP |
| `WorkerService.cs` | Orchestrates scraping, formatting, and email dispatch on a daily schedule |
| `TeamScore.cs` | Model representing a team name and its score |
| `Program.cs` | Dependency injection and host configuration |

---

## Configuration

The application reads email settings from `appsettings.json`. Add the following section:

```json
{
  "EmailSettings": {
    "SmtpServer": "your-smtp-server",
    "Port": 25,
    "SenderEmail": "your-email@example.com",
    "SenderPassword": "your-password",
    "EnableSsl": false
  }
}
```

For local testing, this project was developed using [Papercut SMTP](https://github.com/ChangemakerStudios/Papercut-SMTP) as a local SMTP server.

---

## Email Output Format

Each daily email will contain the game results in the following format:

```
Match 1
----------
Team A: 112
Team B: 98

Match 2
----------
Team C: 105
Team D: 110
```

---

## Dependencies

- [HtmlAgilityPack](https://html-agility-pack.net/) — HTML parsing for scraping
- `Microsoft.Extensions.Hosting` — Background service and DI host
- `Microsoft.Extensions.Configuration` — Configuration binding
- `System.Net.Mail` — SMTP email sending

---

## Getting Started

1. Clone the repository
2. Configure your `appsettings.json` with valid SMTP settings
3. Set the desired send time in `WorkerService.cs` (default: `08:00`)
4. Run the application:

```bash
dotnet run
```

The worker will scrape and send results once per day at the configured time, then wait until the same time the following day.

---

## Notes

- The recipient email address is currently hardcoded in `WorkerService.cs` and can be moved to configuration for flexibility.
- The scraper targets the public box scores page on Basketball Reference. Changes to that site's HTML structure may require updates to the XPath selectors in `ScraperService.cs`.
