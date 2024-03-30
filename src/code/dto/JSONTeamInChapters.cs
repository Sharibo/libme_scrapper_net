using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

// TODO del class
class JSONTeamInChapters {
    // [J("id")]       public long? Id { get; set; }      
    // [J("slug")]     public string Slug { get; set; }   
    // [J("slug_url")] public string SlugUrl { get; set; }
    // [J("model")]    public string Model { get; set; }  
    [J("name")] public string? Name { get; set; }
    [J("cover")] public JSONTeamCover? JSONCover { get; set; }


    public override string ToString() => $"""

                                          Team:
                                          Name      {Name}
                                          Cover:    {PrintJSONCover()}
                                          """;
    
    string PrintJSONCover() {
        return JSONCover
          ?.ToString()
           .Replace("\nCover:", string.Empty)
           .Replace("\n", "\n    ") ?? string.Empty;
    }
    
}