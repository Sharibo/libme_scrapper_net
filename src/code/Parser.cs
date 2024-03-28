using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using libme_scrapper.code.dto;
using Serilog;

namespace libme_scrapper.code;

class Parser {
    
    
    // static Document document;               // TODO 
    // static DocumentCreator documentCreator; // TODO 
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


    // public static List<Chapter> GetTableOfContents(string url) {
    public static void GetTableOfContents(string url) {
        Log.Information("GetTableOfContents started");

        List<Chapter> chaptersList;
        List<Branch> branchesList;
        try {
            HtmlWeb web = new();
            HtmlDocument document;
            
            if (Regex.IsMatch(url, @"^https://ranobelib.me/[A-Za-z0-9-]+\?.*$", RegexOptions.Compiled)) {
                Log.Information("Get first chapter url from main title page");
                document = web.Load(url);
                url = document.DocumentNode.SelectSingleNode(START_READING_BUTTON).GetAttributeValue("href", "");
            }

            document = web.Load(url);
            string rawJSON = document.DocumentNode.SelectSingleNode(CHAPTERS_GETTER).GetDirectInnerText();
            
            if (rawJSON.Equals(string.Empty)) {
                throw new NullReferenceException("rawJSON is empty");
            }

            title = GetTitleName(url);
            string chapters = Regex.Match(rawJSON, @".*""chapters"":(.*),""branches"":.*", RegexOptions.Compiled).Groups[1].Value;
            string branches = Regex.Match(rawJSON, @".*""branches"":(.*),""manga"":.*", RegexOptions.Compiled).Groups[1].Value;
            
            List<JSONChapter> jsonChaptersList = JsonSerializer.Deserialize<List<JSONChapter>>(chapters)
                                          ?? throw new JsonException("Can not deserialize chapters: " + chapters);
            List<JSONBranch> jsonBranchesList = JsonSerializer.Deserialize<List<JSONBranch>>(branches) 
                                         ?? throw new JsonException("Can not deserialize branches: " + branches);

            MapJSONChaptersListToChaptersList(jsonChaptersList, out chaptersList);
            MapJSONBranchesListToBranchesList(jsonBranchesList, out branchesList);
            
        } catch (Exception e) when (e is NullReferenceException or JsonException) {
            Log.Error(e.ToString());
        } catch (Exception e) {
            Log.Error($"{url} {e}");
        }
    
    /*
        // get table of contents
        WebElement chaptersGetter = driver.findElement(By.xpath(CHAPTERS_GETTER));
        new Actions(driver).moveToElement(chaptersGetter).click().perform();
        List<WebElement> chaptersList = driver.findElements(By.className(CHAPTERS));
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
    }

    static void MapJSONChaptersListToChaptersList(List<JSONChapter> jsonChaptersList, out List<Chapter> chapterList) {
        if (jsonChaptersList.Count > 0) {
            chapterList = new();
            foreach (JSONChapter jsonChapter in jsonChaptersList) {
                chapterList.Add(
                    new Chapter(
                        jsonChapter.ChapterName!.Trim(),
                        $"{title}/v{jsonChapter.ChapterVolume}/c{jsonChapter.ChapterNumber}?bid={jsonChapter.BranchId}",
                        jsonChapter.BranchId ??= 1 // TODO разобраться
                    )
                );
            }
        } else {
            throw new NullReferenceException("chaptersJsonList is empty");
        }
    }

    static void MapJSONBranchesListToBranchesList(List<JSONBranch> jsonBranchesList, out List<Branch> branchesList) {
        if (jsonBranchesList.Count > 0) {
            branchesList = new();
            string jsonTeamName = jsonBranchesList[0].Teams.Find(team => team.IsActive == 1).Name; // TODO обдумать
            string jsonTeamIcon = jsonBranchesList[0].Teams.Find(team => team.IsActive == 1).Cover; // TODO обдумать
            foreach (JSONBranch jsonBranch in jsonBranchesList) {
                branchesList.Add(
                    new Branch(
                        jsonBranch.Id ??= 1, // TODO 
                        jsonTeamName,
                        jsonTeamIcon
                    )
                );
            }
        } else {
            throw new NullReferenceException("chaptersJsonList is empty");
        }
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