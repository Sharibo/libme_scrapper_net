using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using libme_scrapper.code.dto;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace libme_scrapper.code;

public class Test {

    public static void Main() {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen)
           .CreateLogger();

        // Log.Verbose("Hello Test!");
        // Log.Debug("Hello Test!");
        // Log.Information("Hello Test!");
        // Log.Warning("Hello Test!");
        // Log.Error("Hello Test!");
        // Log.Fatal("Hello Test!");

        string url = "https://ranobelib.me/ascendance-of-a-bookworm-novel/v1/c0?ui=1709435";
        // string url = "https://ranobelib.me/ascendance-of-a-bookworm-novel?bid=12003&section=chapters&ui=1709435"; // main
        string test1 = "/html/head/script[6]";
        string test2 = "//script[contains(text(), 'window.__DATA__')]";
        // string test2 = "//div[contains(@class, 'media-sidebar__buttons') and contains(@class, 'section')]/a";
        // string test2 = "//div[contains(@class, 'media-sidebar__buttons')][contains(@class, 'section')]/a";
        // string test2 = "//div[class='media-sidebar__buttons' AND class='section']/a";
        try {
            HtmlWeb web = new();
            HtmlDocument? document = web.Load(url);
            string string1 = document.DocumentNode.SelectSingleNode(test2).GetDirectInnerText();
            Log.Information(web.StatusCode.ToString());
            string chapters = string.Empty;
            string branches = string.Empty;
            chapters = Regex.Match(string1, @".*""chapters"":(.*),""branches"":.*", RegexOptions.Compiled).Groups[1].Value;
            branches = Regex.Match(string1, @".*""branches"":(.*),""manga"":.*", RegexOptions.Compiled).Groups[1].Value;
            
            Log.Information(chapters[1..100] + chapters[^100..]);
            
            JsonSerializerOptions jsonOptions = new() { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            // JSONChapter[] chaptersList = JsonSerializer.Deserialize<JSONChapter[]>(chapters) 
                                          // ?? throw new JsonException("Can not deserialize");
            List<JSONChapter> chaptersList = JsonSerializer.Deserialize<List<JSONChapter>>(chapters, jsonOptions)
                                          ?? throw new JsonException("Can not deserialize");
            List<JSONBranch> branchesList = JsonSerializer.Deserialize<List<JSONBranch>>(branches, jsonOptions)
                                          ?? throw new JsonException("Can not deserialize");
            // JSONChaptersList chaptersList = JsonSerializer.Deserialize<JSONChaptersList>(chapters, jsonOptions) 
                                          // ?? throw new JsonException("Can not deserialize");

            for (int index = 0; index < 5; index++) {
                JSONChapter jsonChapter = chaptersList[index];
                Log.Information(jsonChapter.ToString());
            }
            for (int index = 0; index < branchesList.Count; index++) {
                JSONBranch jsonBranch = branchesList[index];
                Log.Information(jsonBranch.ToString());
            }
            // if (chaptersList.chapters != null) {
            //     foreach (JSONChapter jsonChapter in chaptersList.chapters) {
            //         Log.Information(jsonChapter.ToString());
            //     }
            // } else {
            //     Log.Information("ha-ha");
            // }
        }
        catch (Exception e) {
            Log.Error(e.ToString());
            e.HelpLink = url;
            Log.Error(e.HelpLink ?? "without stacktrace");
        }

    }
}