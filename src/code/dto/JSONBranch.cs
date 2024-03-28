using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONBranch {
    [J("id")] public int? Id { get; set; }
    [J("manga_id")] public int? MangaId { get; set; }
    [J("name")] public string? Name { get; set; }
    [J("teams")] public List<JSONTeam>? Teams { get; set; }
    [J("is_subscribed")] public bool? IsSubscribed { get; set; }

    public override string ToString() => $"""

                                          Id            {Id}
                                          MangaId       {MangaId}
                                          Name          {Name}
                                          Teams         {PrintTeams()}
                                          IsSubscribed  {IsSubscribed}
                                          """;

    public string PrintTeams() {
        if (Teams != null) {
            StringBuilder sb = new();
            foreach (JSONTeam team in Teams) {
                sb.Append(team);
            }

            return sb.ToString();
        }

        return string.Empty;
    }
}