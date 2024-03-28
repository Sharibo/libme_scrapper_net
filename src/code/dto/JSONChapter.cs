using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONChapter() {
    
    [J("chapter_id")] public int? ChapterId { get; set; }
    [J("chapter_slug")] public string? ChapterSlug { get; set; }
    [J("chapter_name")] public string? ChapterName { get; set; }
    [J("chapter_number")] public string? ChapterNumber { get; set; } // TODO уточнить
    [J("chapter_volume")] public int? ChapterVolume { get; set; }    // TODO уточнить
    [J("chapter_moderated")] public int? ChapterModerated { get; set; }
    [J("chapter_user_id")] public int? ChapterUserId { get; set; }
    [J("chapter_expired_at")] public string? ChapterExpiredAt { get; set; }
    [J("chapter_scanlator_id")] public int? ChapterScanlatorId { get; set; }
    [J("chapterCreatedAt")] public string? ChapterCreatedAt { get; set; }
    [J("status")] public string? Status { get; set; }
    [J("price")] public int? Price { get; set; }
    [J("branch_id")] public int? BranchId { get; set; }
    [J("username")] public string? Username { get; set; }
    
    public override string ToString() => $"""
                                         
                                         ChapterId          {ChapterId}
                                         ChapterSlug        {ChapterSlug}
                                         ChapterName        {ChapterName}
                                         ChapterNumber      {ChapterNumber}
                                         ChapterVolume      {ChapterVolume}
                                         ChapterModerated   {ChapterModerated}
                                         ChapterUserId      {ChapterUserId}
                                         ChapterExpiredAt   {ChapterExpiredAt}
                                         ChapterScanlatorId {ChapterScanlatorId}
                                         ChapterCreatedAt   {ChapterCreatedAt}
                                         Status             {Status}
                                         Price              {Price}
                                         BranchId           {BranchId}
                                         Username           {Username}
                                         """;
}