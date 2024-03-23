using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;

namespace libme_scrapper.code;

class Parser {
    
    // static Document document;               // TODO 
    // static DocumentCreator documentCreator; // TODO 
    static string? title;                   // TODO 

    const string START_READING_BUTTON = "div.media-sidebar__buttons.section";
    const string CHAPTERS_GETTER = "//div[@data-reader-modal='chapters']";
    const string CHAPTERS = "menu__item";
    const string CHAPTER_CONTAINER = "div.reader-container.container.container_center";
    static readonly string[][] SPELLING_PATTERNS = {
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
    };
    static readonly int[] PAUSES = [100, 150, 200, 250, 300, 350, 400];
    static readonly Random RANDOM = new();


    // public static List<Chapter> GetTableOfContents(string url) {
    //
    //     if (IsMainPageUrl(url)) {
    //         // get firstChapterUrl from main title page
    //         string firstChapterUrl = null;
    //         try {
    //             Document document = Jsoup.connect(url)
    //                     .userAgent("Mozilla/5.0")
    //                     .get();
    //             Element data = document.select(START_READING_BUTTON).get(0).children().get(1);
    //             firstChapterUrl = Optional.of(data.attr("href"))
    //                     .orElseThrow(() -> new RuntimeException("firstChapterUrl was not found!"));
    //         } catch (IOException e) {
    //             log.error(url + "\n" + e.getLocalizedMessage());
    //         }
    //
    //         url = firstChapterUrl;
    //     }
    //
    //     // create a browser
    //     EdgeOptions options = new EdgeOptions();
    //     options.addArguments("--headless=new");
    //     options.addArguments("--remote-allow-origins=*");
    //     EdgeDriver driver = new EdgeDriver(options);
    //     Dimension dimension = new Dimension(1920, 1080);
    //     driver.manage().window().setSize(dimension);
    //
    //     // get url from TextField
    //     driver.navigate().to(url);
    //
    //     // get table of contents
    //     WebElement chaptersGetter = driver.findElement(By.xpath(CHAPTERS_GETTER));
    //     new Actions(driver).moveToElement(chaptersGetter).click().perform();
    //     List<WebElement> chaptersList = driver.findElements(By.className(CHAPTERS));
    //     List<Chapter> tableOfContents = new ArrayList<>(chaptersList.size());
    //     chaptersList.forEach(element ->
    //             tableOfContents.add(new Chapter(element.getText(), element.getAttribute("href")))
    //     );
    //     Collections.reverse(tableOfContents);
    //
    //     title = GetTitleName(tableOfContents.get(0).getChapterLink());
    //
    //     log.info("Table of content downloaded successfully");
    //     driver.quit();
    //
    //     return tableOfContents;
    // }
    //
    // private static boolean IsMainPageUrl(string url) {
    //     Pattern p = Pattern.compile("^https://ranobelib.me/[A-Za-z0-9-]+\\?.*$");
    //     Matcher m = p.matcher(url);
    //     return  m.find();
    // }
    //
    // private static string GetTitleName(string url) {
    //     Pattern p = Pattern.compile("ranobelib.me/([A-Za-z0-9-]+)/");
    //     Matcher m = p.matcher(url);
    //     if (m.find()) {
    //         string capitalized = m.group(1).substring(0, 1).toUpperCase() + m.group(1).substring(1);
    //         if (capitalized.matches("^[A-Za-z0-9-]+novel$")) {
    //             capitalized = capitalized.substring(0, capitalized.length() - 6);
    //         }
    //         return capitalized.replaceAll("-", " ");
    //     } else {
    //         log.error("Title name not found in url: " + url);
    //         return "Unknown";
    //     }
    // }
    //
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
    //     log.info("Load: " + chapter.getChapterName());
    //     try {
    //         document = Jsoup.connect(chapter.getChapterLink())
    //                 .userAgent("Mozilla/5.0")
    //                 .get();
    //         Elements data = document.select(CHAPTER_CONTAINER).get(0).children();
    //         data.forEach(element -> parseData(element));
    //     } catch (IOException e) {
    //         log.error(chapter.getChapterName() + e.getLocalizedMessage());
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
    //                     () -> log.error("Image not found! " + element.outerHtml()));
    //             break;
    //         default:
    //             log.warn(element.tagName() + " cannot be parsed");
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