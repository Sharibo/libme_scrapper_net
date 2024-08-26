using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONBranchInChapters {
    // [J("id")] public int? Id { get; set; }
    [J("branch_id")] public int? BranchId { get; set; }
    // [J("created_at")] public string? CreatedAt { get; set; }
    // [J("teams")] public List<JSONTeamInChapters>? JSONTeams { get; set; }
    // [J("user")]       public User User { get; set; }       


    public override string ToString() => $"""
                                          
                                          Branch:
                                          BranchId  {BranchId?.ToString() ?? "null"}
                                          """;

    // string PrintJSONTeams() {
    //     if (JSONTeams?.Count > 0) {
    //         StringBuilder sb = new();
    //         foreach (JSONTeamInChapters team in JSONTeams) {
    //             sb.Append(team);
    //         }
    //
    //         sb.Replace("\n", "\n    ");
    //         return sb.ToString();
    //     }
    //
    //     return string.Empty;
    // }
}