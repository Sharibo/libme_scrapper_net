using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using libme_scrapper.code.dto;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace libme_scrapper.code;

public partial class Test {

    static string title = string.Empty;
    static string apiBody = string.Empty;
    static readonly HttpClient WEB = new(
        new SocketsHttpHandler {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            MaxConnectionsPerServer = 50,
        }
    ) {
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact,
        BaseAddress = new Uri("https://api.lib.social/api/manga/"),
    };

    // asdkjqwdjlk
    public static void Main() {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen)
           .CreateLogger();

        string url = "https://ranobelib.me/ascendance-of-a-bookworm-novel/v1/c0?ui=1709435";
        string url2 = "https://ranobelib.me/ascendance-of-a-bookworm-novel?section=info&ui=1709435";
        string url3 = "https://test-front.ranobelib.me/ru/manga/6689--ascendance-of-a-bookworm-novel?from=catalog";
        // string url = "https://ranobelib.me/ascendance-of-a-bookworm-novel?bid=12003&section=chapters&ui=1709435"; // main
        string test1 = "/html/head/script[6]";
        string test2 = "//script[contains(text(), 'window.__DATA__')]";
        // string test2 = "//div[contains(@class, 'media-sidebar__buttons') and contains(@class, 'section')]/a";
        // string test2 = "//div[contains(@class, 'media-sidebar__buttons')][contains(@class, 'section')]/a";
        // string test2 = "//div[class='media-sidebar__buttons' AND class='section']/a";
        try {

            HttpClient WEB2 = new(
                new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
            ) {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                BaseAddress = new Uri("https://api.lib.social/api/manga/"),
            };

            Log.Information(WEB.BaseAddress?.ToString()!);
            SetApiBody(url2);
            SetApiBody(url3);
            Log.Information("out " + title);
            // HtmlWeb web = new();
            // HtmlDocument? document = web.Load(url);
            // string string1 = document.DocumentNode.SelectSingleNode(test2).GetDirectInnerText();
            // Log.Information(web.StatusCode.ToString());
            // string chapters = string.Empty;
            // string branches = string.Empty;
            // chapters = Regex.Match(string1, @".*""chapters"":(.*),""branches"":.*", RegexOptions.Compiled).Groups[1].Value;
            // branches = Regex.Match(string1, @".*""branches"":(.*),""manga"":.*", RegexOptions.Compiled).Groups[1].Value;
            //
            // Log.Information(chapters[1..100] + chapters[^100..]);
            //
            // JsonSerializerOptions jsonOptions = new() { 
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //     WriteIndented = true
            // };
            //
            // // JSONChapter[] chaptersList = JsonSerializer.Deserialize<JSONChapter[]>(chapters) 
            //                               // ?? throw new JsonException("Can not deserialize");
            // List<JSONChapter> chaptersList = JsonSerializer.Deserialize<List<JSONChapter>>(chapters, jsonOptions)
            //                               ?? throw new JsonException("Can not deserialize");
            // List<JSONBranchInChapters> branchesList = JsonSerializer.Deserialize<List<JSONBranchInChapters>>(branches, jsonOptions)
            //                               ?? throw new JsonException("Can not deserialize");
            // // JSONChapters chaptersList = JsonSerializer.Deserialize<JSONChapters>(chapters, jsonOptions) 
            //                               // ?? throw new JsonException("Can not deserialize");
            //
            // for (int index = 0; index < 5; index++) {
            //     JSONChapter jsonChapter = chaptersList[index];
            //     Log.Information(jsonChapter.ToString());
            // }
            // for (int index = 0; index < branchesList.Count; index++) {
            //     JSONBranchInChapters jsonBranch = branchesList[index];
            //     Log.Information(jsonBranch.ToString());
            // }
            // // if (chaptersList.chapters != null) {
            // //     foreach (JSONChapter jsonChapter in chaptersList.chapters) {
            // //         Log.Information(jsonChapter.ToString());
            // //     }
            // // } else {
            // //     Log.Information("ha-ha");
            // // }
        } catch (Exception e) {
            Log.Error(e.ToString());
            e.HelpLink = url;
            Log.Error(e.HelpLink ?? "without stacktrace");
        }

    }


    static void SetApiBody(string url) {
        string legacyUrl = Regex.Match(url, @"^https://ranobelib.me/([A-Za-z0-9-]+)(\?*.*|/.+)$", RegexOptions.Compiled).Groups[1].Value;
        Log.Information("legacyUrl " + legacyUrl);

        if (legacyUrl != string.Empty) {
            Log.Information("Fix legacy first chapter url from main title page");
            using HttpResponseMessage response = WEB.GetAsync(legacyUrl).WaitAsync(TimeSpan.FromSeconds(5)).Result;
            string json = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().WaitAsync(CancellationToken.None).Result;
            apiBody = Regex.Match(json, "\"slug_url\":\"([^\"]+)\",", RegexOptions.Compiled).Groups[1].Value;
            // Log.Information(json);
            Log.Information(apiBody);
            title = Regex.Unescape(Regex.Match(json, "\"rus_name\":\"([^\"]+)\",", RegexOptions.Compiled).Groups[1].Value);
            Log.Information(title);
            bool tr = MyRegex().IsMatch(title);
            Log.Information(tr.ToString());
            title = Regex.Replace(title, @" [(]{0,1}(веб|веб[ -]{0,1}версия|веб[ -]{0,1}[новела]{5,8}|[новела]{5,8}|[лайт]{3,5}[ -]{0,1}[новела]{5,8}|лн|вн|web|web[ -]{0,1}[version]{6,8}|web[ -]{0,1}[novel]{4,6}|ln|wn|[novel]{4,6}|[light]{4,6}[ -]{0,1}[novel]{4,6})[)]{0,1}$", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase).Trim();
            Log.Information(title);
        } else {
            apiBody = Regex.Match(url, @"^https://test-front.ranobelib.me/ru/manga/([A-Za-z0-9-]+)\?*.*$", RegexOptions.Compiled).Groups[1].Value;
            if (apiBody == string.Empty) {
                throw new NullReferenceException("apiBody is null");
            }

            Log.Information("New test-front version");
            Log.Information(apiBody);
        }

    }

    [GeneratedRegex(" [(]{0,1}(веб|веб[ -]{0,1}версия|веб[ -]{0,1}[новела]{5,8}|[новела]{5,8}|[лайт]{3,5}[ -]{0,1}[новела]{5,8}|лн|вн|web|web[ -]{0,1}[version]{6,8}|web[ -]{0,1}[novel]{4,6}|ln|wn|[novel]{4,6}|[light]{4,6}[ -]{0,1}[novel]{4,6})[)]{0,1}$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex MyRegex();
}