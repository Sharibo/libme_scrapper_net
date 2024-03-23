namespace libme_scrapper.code;

class Chapter {
    public string ChapterName { get; set; }
    public string ChapterLink { get; set; }

    public Chapter(string chapterName, string chapterLink) {
        ChapterName = chapterName;
        ChapterLink = chapterLink;
    }

    public override string ToString() => $"chapterName = {ChapterName}\n";
}