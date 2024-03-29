using System.Collections.Generic;
using System.Text;
using Serilog;
using static libme_scrapper.code.dto.Nesting;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONChapter() {
    // [J("id")] public int? Id { get; set; }
    [J("index")] public int? Index { get; set; } // TODO для сортировки или выкинуть
    // [J("item_number")] public int? ItemNumber { get; set; }
    [J("volume")] public string? Volume { get; set; }
    [J("number")] public string? Number { get; set; }
    // [J("number_secondary")] public string NumberSecondary { get; set; }
    [J("name")] public string? Name { get; set; }
    [J("branches_count")] public int? BranchesCount { get; set; }   // TODO подумать
    [J("branches")] public List<JSONBranch>? JSONBranches { get; set; } // TODO toString todo

    public override string ToString() => $"""
                                          
                                          Chapter:
                                          Index         {Index}
                                          Volume        {Volume}
                                          Number        {Number}
                                          Name          {Name}
                                          BranchesCount {BranchesCount}
                                          JSONBranches  {PrintJSONBranches()}
                                          """;
    
    string PrintJSONBranches() {
        if (JSONBranches?.Count > 0) {
            StringBuilder sb = new();
            foreach (JSONBranch branch in JSONBranches) {
                sb.Append('\n').Append(branch);
            }
            
            sb.Replace("\n", "\n    ");
            return sb.ToString();
        }

        return string.Empty;
    }
}