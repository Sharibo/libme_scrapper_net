using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONTeamCover {
    // [J("filename")] public string Filename { get; set; }
    [J("thumbnail")] public string? Thumbnail { get; set; } // TODO не нужно?
    [J("default")] public string? Default { get; set; }
    
    public override string ToString() => $"""
                                          
                                          Cover:
                                          Thumbnail {Thumbnail}
                                          Default   {Default}
                                          """;
}