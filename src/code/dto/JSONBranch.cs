using System.Collections.Generic;
using System.Text;
using Serilog;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONBranch {
    [J("id")] public long? Id { get; set; }
    [J("branch_id")] public long? BranchId { get; set; }
    // [J("created_at")] public string? CreatedAt { get; set; }
    [J("teams")] public List<JSONTeam>? JSONTeams { get; set; }
    // [J("user")]       public User User { get; set; }       


    public override string ToString() => $"""
                                          
                                          Branch:
                                          Id        {Id}
                                          MangaId   {BranchId}
                                          Teams     {PrintJSONTeams()}
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