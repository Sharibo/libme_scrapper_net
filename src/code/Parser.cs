using libme_scrapper.code.dto;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace libme_scrapper.code;

class Parser {

    static readonly ILogger LOG = Log.ForContext<Parser>();

    // static Document document;               // TODO 
    // static DocumentCreator documentCreator; // TODO 
    const string API = "https://api.lib.social/api/";
    const string API_SITE = "https://ranobelib.me/";
    const string API_RU_PREFIX = "ru/";
    const string API_BOOK_PREFIX = "book/";
    const string API_MANGA_PREFIX = "manga/";
    const string API_FULL_MAIN = "?fields[]=authors";
    const string API_BRANCHES_PREFIX = "branches/";
    const string API_BRANCHES = "?team_defaults=1";
    const string API_CHAPTERS = "/chapters";
    static string apiTitle = string.Empty;
    static string title = string.Empty;

    const string START_READING_BUTTON = "//div[contains(@class, 'media-sidebar__buttons') and contains(@class, 'section')]/a";
    const string CHAPTERS_GETTER = "//script[contains(text(), 'window.__DATA__')]";
    static string chapters = string.Empty;
    static string jsonChapters = string.Empty;
    const string CHAPTER_CONTAINER = "div.reader-container.container.container_center";
    static readonly string[][] SPELLING_PATTERNS = [
            // doubleSpace
            [@"[\u00A0\s]{{2,}}", " "],
            // dash 1
            [@"[\u00A0\s][-–—]{1,2}([^\u00A0\s])", " —\u00A0$1"],
            // dash 2
            [@"([^\u00A0\s])[-–—]{1,2}[\u00A0\s]", "$1 —\u00A0"],
            // dash 3
            [@"^[-–—]{1,2}[\u00A0\s]?([^\u00A0\s])", "—\u00A0$1"],
            // dot, question mark, exclamation mark, comma, semicolon with symbol
            [@"([\.,\?!;])([^\u00A0\s\.,\?!""'‚‘‛’‟„“”‹›«»「」『』(){}\[\]])", "$1 $2"],
            // quotes and dots 1
            [@"\.([""'‚‘‛’‟„“”‹›«»「」『』〈〉《》(){}\[\]])", "$1."],
            // quotes and dots 2
            [@"\.\.([""'‚‘‛’‟„“”‹›«»「」『』〈〉《》(){}\[\]])\.", "...$1"],
            // little spruces 1
            [@"[""'‚‘‛’‟„“”‹›«»]([^\u00A0\s])", "«$1"],  // 「」『』〈〉《》 сомнительно
            // little spruces 2
            [@"([^\u00A0\s])[""'‚‘‛’‟„“”‹›«»]", "$1»"],
            // little spruces 3
            [@"[""'‚‘‛’‟„“”‹›«»]{2}([^\u00A0\s])", "««$1"],
            // little spruces 4
            [@"([^\u00A0\s])[""'‚‘‛’‟„“”‹›«»]{2}", "$1»»"],
    ];
    static readonly int[] PAUSES = [100, 150, 200, 250, 300, 350, 400];
    static readonly Random RANDOM = new();
    // static readonly HtmlWeb WEB = new();
    static readonly HttpClient WEB = new(
        new SocketsHttpHandler {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            MaxConnectionsPerServer = 50,
        }
    ) {
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact,
        BaseAddress = new Uri(API),
    };

    public static void Main() {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen,
                applyThemeToRedirectedOutput: true
                )
           .CreateLogger();

        IList<Branch> list = GetTableOfContents("https://ranobelib.me/ru/book/39562--otonari-no-tenshi-sama-ni-itsu-no-ma-ni-ka-dame-ningen-ni-sareteita-ken-wn?section=chapters");
        // IList<Branch> list = GetTableOfContents("https://ranobelib.me/ru/book/6689--ascendance-of-a-bookworm-novel?from=catalog&ui=1709435");
        //foreach (Branch branch in list) {
        //    Console.WriteLine(branch.ToString());
        //    Log.Information(branch.ToString());
        //}
    }

    public static IList<Branch> GetTableOfContents(string url) {
        Log.Information("GetTableOfContents started");

        url = "https://ranobelib.me/ru/book/39562--otonari-no-tenshi-sama-ni-itsu-no-ma-ni-ka-dame-ningen-ni-sareteita-ken-wn?section=chapters";

        IList<Branch> branchesList = null!;
        try {
            SetApiTitleAndTitle(url);
            branchesList = GetBranches();
            IList<Chapter> chaptersList = GetChapters();
            FillBranches(ref branchesList, ref chaptersList);
        } catch (Exception e) when (e is NullReferenceException or JsonException) {
            Log.Error(e.ToString());
        } catch (ArgumentNullException e) { // todo
            Log.Error($"{url} {e}");
        } catch (Exception e) {
            Log.Error($"{url} {e}"); // TODO api and more
        }

        /*
            // get table of contents
            WEBElement chaptersGetter = driver.findElement(By.xpath(CHAPTERS_GETTER));
            new Actions(driver).moveToElement(chaptersGetter).click().perform();
            List<WEBElement> chaptersList = driver.findElements(By.className(CHAPTERS));
            List<Chapter> tableOfContents = new ArrayList<>(chaptersList.size());
            chaptersList.forEach(element ->
                    tableOfContents.add(new Chapter(element.getText(), element.getAttribute("href")))
            );
            Collections.reverse(tableOfContents);

            title = GetTitleName(tableOfContents.get(0).getChapterLink());

            Log.Information("Table of content downloaded successfully");
            driver.quit();

            return tableOfContents;
            */
        Log.Information("GetTableOfContents finished");
        return branchesList;
    }

    static void FillBranches(ref IList<Branch> branchesList, ref IList<Chapter> chaptersList) {
        Dictionary<int, int> ids = new();
        for (int index = 0; index < branchesList.Count; index++) {
            ids.Add(branchesList[index].Id, index);
        }

        foreach (Chapter chapter in chaptersList) {
            branchesList[ids[chapter.BranchId]].Chapters.Add(chapter);
        }
    }

    static List<Branch> GetBranches() {
        ReadOnlySpan<char> apiTitleId = apiTitle.AsSpan(0..apiTitle.IndexOf('-'));
        Log.Information($"apiTitleId {apiTitleId}");

        using HttpResponseMessage response = WEB.GetAsync($"{API_BRANCHES_PREFIX}{apiTitleId}{API_BRANCHES}")
           .WaitAsync(TimeSpan.FromSeconds(3)).Result;
        string json = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().WaitAsync(TimeSpan.FromSeconds(3)).Result;

        if (json.Equals(string.Empty)) {
            throw new NullReferenceException("branches json is empty");
        }

        JSONBranchesList jsonBranchesList = JsonSerializer.Deserialize<JSONBranchesList>(json)
                                         ?? throw new JsonException("Can not deserialize branches list: " + json);

        Log.Information("jsonBranchesList");
        Log.Information(jsonBranchesList.ToString());
        return MapJSONBranchesListToBranchesList(jsonBranchesList);
    }

    static List<Branch> MapJSONBranchesListToBranchesList(JSONBranchesList jsonBranchesList) {
        List<Branch> branchesList = new();
        foreach (JSONBranch jsonBranch in jsonBranchesList.JSONBranches ??
                                          throw new NullReferenceException("jsonBranchesList.JSONBranches is empty")) {
            branchesList.Add(new Branch(jsonBranch));
        }

        if (branchesList.Count == 0) {
            branchesList.Add(new Branch());
        }

        return branchesList;
    }

    static List<Chapter> GetChapters() {
        using HttpResponseMessage response = WEB.GetAsync($"{API_MANGA_PREFIX}{apiTitle}{API_CHAPTERS}").WaitAsync(TimeSpan.FromSeconds(3)).Result;
        string json = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().WaitAsync(TimeSpan.FromSeconds(5)).Result;

        if (json.Equals(string.Empty)) {
            throw new NullReferenceException("chapters json is empty");
        }

        JSONChaptersList jsonChaptersList = JsonSerializer.Deserialize<JSONChaptersList>(json)
                                         ?? throw new JsonException("Can not deserialize chapters list: " + json);

        Log.Information(jsonChaptersList.ToString());
        return MapJSONChaptersListToChaptersList(jsonChaptersList);
    }

    static List<Chapter> MapJSONChaptersListToChaptersList(JSONChaptersList jsonChaptersList) {
        List<Chapter> chaptersList = new();
        foreach (JSONChapter jsonChapter in jsonChaptersList.JSONChapters ??
                                            throw new NullReferenceException("jsonChaptersList.JSONChapters is empty")) {

            if (jsonChapter.BranchesCount != null) {
                if (jsonChapter.BranchesCount.Value > 1) {
                    for (int i = 0; i < jsonChapter.BranchesCount; i++) {
                        chaptersList.Add(new Chapter(jsonChapter, i));
                        LOG.Information("in for");
                    }
                } else {
                    LOG.Information("in else");
                    chaptersList.Add(new Chapter(jsonChapter, 0));
                }
            } else {
                throw new NullReferenceException("jsonChapter.BranchesCount is null");
            }
        }

        return chaptersList;
    }

    static void SetApiTitleAndTitle(string url) {
        // string apiTitleLegacy = Regex.Match(url, @"^https://ranobelib.me/([A-Za-z0-9-]+)(\?*.*|/.+)$", RegexOptions.Compiled).Groups[1].Value;
        // Log.Information("apiTitleLegacy " + apiTitleLegacy);

        // if (apiTitleLegacy != string.Empty) {
        //     Log.Information("Legacy front version");
        //     using HttpResponseMessage response = WEB.GetAsync($"{API_MANGA_PREFIX}{apiTitleLegacy}").WaitAsync(TimeSpan.FromSeconds(3)).Result;
        //     string json = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().WaitAsync(TimeSpan.FromSeconds(3)).Result;

        //     apiTitle = Regex.Match(json, "\"slug_url\":\"([^\"]+)\",", RegexOptions.Compiled).Groups[1].Value;
        //     if (apiTitle == string.Empty) {
        //         throw new NullReferenceException("apiTitle is null");
        //     }

        //     title = Regex.Unescape(Regex.Match(json, "\"rus_name\":\"([^\"]+)\",", RegexOptions.Compiled).Groups[1].Value);
        //     title = Regex.Replace(title,
        //         @" [(]{0,1}(веб|веб[ -]{0,1}версия|веб[ -]{0,1}[новела]{5,8}|[новела]{5,8}|[лайт]{3,5}[ -]{0,1}[новела]{5,8}|лн|вн|web|web[ -]{0,1}[version]{6,8}|web[ -]{0,1}[novel]{4,6}|ln|wn|[novel]{4,6}|[light]{4,6}[ -]{0,1}[novel]{4,6})[)]{0,1}$",
        //         string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase).Trim();
        // } else {
        apiTitle = Regex.Match(url, @$"^{API_SITE}{API_RU_PREFIX}{API_BOOK_PREFIX}([A-Za-z0-9-]+)\?*.*$", RegexOptions.Compiled).Groups[1].Value;
        if (apiTitle == string.Empty) {
            apiTitle = Regex.Match(url, @$"^{API_SITE}{API_RU_PREFIX}([A-Za-z0-9-]+)/.*$", RegexOptions.Compiled).Groups[1].Value;
            throw new NullReferenceException("apiTitle is null");
        }

        Log.Information($"apiTitle: {apiTitle}");

        using HttpResponseMessage response = WEB.GetAsync($"{API_MANGA_PREFIX}{apiTitle}").WaitAsync(TimeSpan.FromSeconds(3)).Result;
        string json = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().WaitAsync(TimeSpan.FromSeconds(3)).Result;

        title = Regex.Unescape(Regex.Match(json, "\"rus_name\":\"([^\"]+)\",", RegexOptions.Compiled).Groups[1].Value);
        Log.Information($"title raw: {title}");
        // title = Regex.Replace(title,
        //     @"[(]{0,1}(веб|веб[ -]{0,1}версия|веб[ -]{0,1}[новела]{5,8}|[новела]{5,8}|[лайт]{3,5}[ -]{0,1}[новела]{5,8}|лн|вн|web|web[ -]{0,1}[version]{6,8}|web[ -]{0,1}[novel]{4,6}|ln|wn|[novel]{4,6}|[light]{4,6}[ -]{0,1}[novel]{4,6})[)]{0,1}$",
        //     string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase).Trim();
        title = Regex.Replace(title,
            @"([(]?(веб([ -]?(версия|[новела]{5,8}))?|([лайт]{3,5}[ -]?)?[новела]{5,8}|лн|вн|web([ -]?([version]{6,8}|[novel]{4,6}))?|ln|wn|([light]{4,6}[ -]?)?[novel]{4,6})[)]? ?)*$",
            string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase).Trim();
        // }

        Log.Information($"title: {title}");
    }


    static bool IsMainPageUrl(string url) {
        return Regex.IsMatch(url, @"^https://ranobelib.me/[A-Za-z0-9-]+\?.*$", RegexOptions.Compiled);
    }

    static string GetTitleName(string url) {
        string title = Regex.Match(url, "ranobelib.me/(.+)/.*", RegexOptions.Compiled).Value;

        if (title.Equals(string.Empty)) {
            throw new NullReferenceException("Title name not found in url: " + url);
        }

        string capitalized = string.Concat(title[0].ToString().ToUpper(), title.AsSpan(1));
        if (Regex.IsMatch(capitalized, "^.+novel$", RegexOptions.Compiled)) {
            capitalized = capitalized[..^6];
        }

        return capitalized.Replace("-", " ");
    }

    // public static void GetData(List<Chapter> checkedChapters) {
    //     documentCreator = new DocumentCreator();
    //     documentCreator.createDocument(title);
    //     GetChapters(checkedChapters);
    // }
    //
    // public static void GetData(List<Chapter> checkedChapters, int nChaptersPart) {
    //     documentCreator = new DocumentCreator();
    //     documentCreator.createDocument("%s - часть %s".formatted(title, nChaptersPart));
    //     GetChapters(checkedChapters);
    // }
    //
    // public static void GetData(List<Chapter> checkedChapters, string volumeNumber) {
    //     documentCreator = new DocumentCreator();
    //     documentCreator.createDocument("Том %s %s".formatted(volumeNumber, title));
    //     GetChapters(checkedChapters);
    // }
    //
    // public static void GetData(List<Chapter> checkedChapters, string volumeNumber, int nChaptersPart) {
    //     documentCreator = new DocumentCreator();
    //     documentCreator.createDocument("Том %s часть %s %s".formatted(volumeNumber, nChaptersPart, title));
    //     GetChapters(checkedChapters);
    // }
    //
    // // to index is included
    // private static void GetChapters(List<Chapter> checkedChapters) {
    //     for (int i = 0, chaptersSize = checkedChapters.size() - 1; i < chaptersSize; i++) {
    //         Chapter chapter = checkedChapters.get(i);
    //         documentCreator.addChapterName(chapter.getChapterName());
    //         GetChapter(chapter);
    //         documentCreator.addPageBreak();
    //         Pause();
    //     }
    //
    //     // last chapter without page break
    //     Chapter chapter = checkedChapters.get(checkedChapters.size() - 1);
    //     documentCreator.addChapterName(chapter.getChapterName());
    //     GetChapter(chapter);
    // }
    //
    // private static void GetChapter(Chapter chapter) {
    //     Log.Information("Load: " + chapter.getChapterName());
    //     try {
    //         document = Jsoup.connect(chapter.getChapterLink())
    //                 .userAgent("Mozilla/5.0")
    //                 .get();
    //         Elements data = document.select(CHAPTER_CONTAINER).get(0).children();
    //         data.forEach(element -> parseData(element));
    //     } catch (IOException e) {
    //         Log.Error(chapter.getChapterName() + e.getLocalizedMessage());
    //     }
    // }
    //
    // private static void ParseData(Element element) {
    //     switch (element.tagName()) {
    //         case "p":
    //             documentCreator.addTextParagraph(CheckSpelling(element.text()));
    //             break;
    //         case "div":
    //             Optional<string> source = Optional.of(element.firstElementChild().attr("data-src"));
    //             source.ifPresentOrElse(url -> documentCreator.addImgParagraph(url),
    //                     () -> Log.Error("Image not found! " + element.outerHtml()));
    //             break;
    //         default:
    //             Log.warn(element.tagName() + " cannot be parsed");
    //     }
    // }
    //
    // private static string CheckSpelling(string text) {
    //     string trimmedText = text.strip();
    //
    //     if (trimmedText.contains("http")) {
    //         return trimmedText;
    //     }
    //
    //     for (string[] pattern : SPELLING_PATTERNS) {
    //         trimmedText = trimmedText.replaceAll(pattern[0], pattern[1]); //TODO всё же попытаться на стрингбилдер переписать
    //     }
    //
    //     return trimmedText;
    // }
    //
    // static void Pause() {
    //     Thread.Sleep(PAUSES[RANDOM.Next(PAUSES.Length)]);
    // }
    //
    // public static string SaveDocument(string pathToSave) {
    //     if (documentCreator.saveDocument(pathToSave, title)) {
    //         return "Сохранено!";
    //     } else {
    //         return "Возникла ошибка при сохранении!";
    //     }
    // }
    //
    // public static string SaveDocument(string pathToSave, int nChaptersPart) {
    //     if (documentCreator.saveDocument(pathToSave, "%s - часть %s".formatted(title, nChaptersPart))) {
    //         return "Сохранена часть " + nChaptersPart + "!";
    //     } else {
    //         return "Возникла ошибка при сохранении!";
    //     }
    // }
    //
    // public static string SaveDocument(string pathToSave, string volumeNumber) {
    //     if (documentCreator.saveDocument(pathToSave, "Том %s %s".formatted(volumeNumber, title))) {
    //         return "Том " + volumeNumber + " сохранён!";
    //     } else {
    //         return "Возникла ошибка при сохранении!";
    //     }
    // }
    //
    // public static string SaveDocument(string pathToSave, string volumeNumber, int nChaptersPart) {
    //     if (documentCreator.saveDocument(pathToSave, "Том %s часть %s %s".formatted(volumeNumber, nChaptersPart, title))) {
    //         return "Том %s часть %s сохранён!".formatted(volumeNumber, nChaptersPart);
    //     } else {
    //         return "Возникла ошибка при сохранении!";
    //     }
    // }

}