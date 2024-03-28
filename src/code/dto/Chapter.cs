namespace libme_scrapper.code.dto;

class Chapter(string chapterName, string chapterLink, int branchId) {
    public string ChapterName { get; set; } = chapterName;
    public string ChapterLink { get; set; } = chapterLink;
    public int BranchId { get; set; } = branchId;

    public override string ToString() => $"chapterName = {ChapterName}\n";
}