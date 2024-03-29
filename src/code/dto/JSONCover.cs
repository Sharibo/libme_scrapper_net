using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONCover {
    // [J("filename")] public string Filename { get; set; }
    [J("thumbnail")] public string? Thumbnail { get; set; }
    [J("default")] public string? Default { get; set; }
    
    public override string ToString() => $"""
                                          
                                          Thumbnail {Thumbnail}
                                          Default   {Default}
                                          """;
}