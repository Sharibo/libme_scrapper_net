using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONBranchesList {
    [J("data")] public List<JSONBranch>? JSONBranches { get; set; }

    
    public override string ToString() => $"""

                                          JSONBranches: {PrintJSONBranches()}
                                          """;
    
    string PrintJSONBranches() {
        if (JSONBranches?.Count > 0) {
            StringBuilder sb = new();
            foreach (JSONBranch branch in JSONBranches) {
                sb.Append(branch);
            }
            
            sb.Replace("\n", "\n    ");
            return sb.ToString();
        }

        return string.Empty;
    }
}