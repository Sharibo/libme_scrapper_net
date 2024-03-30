using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONBranch {
    [J("id")] public int? Id { get; set; }
    [J("name")] public string? Name { get; set; }
    // [J("notify")] public bool? Notify { get; set; }    
    [J("teams")] public List<JSONTeam>? JSONTeams { get; set; }


    public override string ToString() => $"""

                                          Branch:
                                          Id        {Id}
                                          MangaId   {Name}
                                          Teams:    {PrintJSONTeams()}
                                          """;

    string PrintJSONTeams() {
        if (JSONTeams?.Count > 0) {
            StringBuilder sb = new();
            foreach (JSONTeam team in JSONTeams) {
                sb.Append(team);
            }

            sb.Replace("\n", "\n    ");
            return sb.ToString();
        }

        return string.Empty;
    }
}