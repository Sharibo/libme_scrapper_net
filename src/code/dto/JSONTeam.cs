using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

public class JSONTeam {
    // [J("id")]       public long? Id { get; set; }      
    // [J("slug")]     public string Slug { get; set; }   
    // [J("slug_url")] public string SlugUrl { get; set; }
    // [J("model")]    public string Model { get; set; }  
    [J("name")] public string? Name { get; set; }
    [J("cover")] internal JSONCover? JSONCover { get; set; }   

    
    public override string ToString() => $"""
                                          
                                          Name  {Name}
                                          Cover {PrintJSONCover()}
                                          """;
    
    string PrintJSONCover() {
        return JSONCover?.ToString()?.Replace("\n", "\n    ") ?? string.Empty;
    }
    
}