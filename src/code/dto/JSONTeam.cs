using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONTeam {
    [J("id")] public int? Id { get; set; } // TODO не нужно
    // [J("slug")]     public string? Slug { get; set; }    
    // [J("slug_url")] public string? SlugUrl { get; set; } 
    // [J("model")]    public string? Model { get; set; }   
    [J("name")]     public string? Name { get; set; }    
    [J("cover")]    public JSONTeamCover? JSONCover { get; set; }    
    [J("details")]  public JSONTeamDetails? JSONDetails { get; set; }

    
    
    public override string ToString() => $"""

                                          Team:
                                          Id        {Id}
                                          Name      {Name}
                                          Cover:    {PrintJSONCover()}
                                          Details:  {PrintJSONDetails()}
                                          """;
    
    string PrintJSONCover() {
        return JSONCover
          ?.ToString()
           .Replace("\nCover:", string.Empty)
           .Replace("\n", "\n    ") ?? string.Empty;
    }
    
    string PrintJSONDetails() {
        return JSONDetails
          ?.ToString()
           .Replace("\nDetails:", string.Empty)
           .Replace("\n", "\n    ") ?? string.Empty;
    }
}