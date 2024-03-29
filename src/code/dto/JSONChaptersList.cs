using System.Collections.Generic;
using System.Text;
using Serilog;
using static libme_scrapper.code.dto.Nesting;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONChaptersList {
    [J("data")] public List<JSONChapter>? JSONChapters { get; set; }
    
    public override string ToString() => $"""
                                          
                                          JSONChapters  {PrintJSONChapters()}
                                          """;
    
    string PrintJSONChapters() {
        if (JSONChapters?.Count > 0) {
            StringBuilder sb = new();
            // foreach (JSONChapter chapter in JSONChapters) { // TODO вернуть
                // sb.Append(chapter);
            // }
            for (int index = 0; index < 5; index++) {
                JSONChapter chapter = JSONChapters[index];
                sb.Append('\n').Append(chapter);
            }
            
            sb.Replace("\n", "\n    ");
            return sb.ToString();
        }

        return string.Empty;
    }
    
}