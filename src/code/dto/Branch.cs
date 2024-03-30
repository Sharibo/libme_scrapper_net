using System;
using System.Collections.Generic;
using System.Text;

namespace libme_scrapper.code.dto;

class Branch {
    // public int Index { get; set; }
    public int Id { get; set; }
    public string TeamName { get; set; } = null!;
    public string TeamIcon { get; set; } = null!; // TODO set -> init? во всех свойствах
    public List<Chapter> Chapters { get; set; } = new();

    public Branch(JSONBranch jsonBranch) {
        Id = jsonBranch.Id ?? throw new NullReferenceException("branch id is null");
        
        foreach (JSONTeam jsonTeam in jsonBranch.JSONTeams ?? throw new NullReferenceException("teams list is null")) {
            if (jsonTeam.JSONDetails?.IsActive == true) {
                TeamName = jsonTeam.Name ?? throw new NullReferenceException("team name is null");
                TeamIcon = jsonTeam.JSONCover?.Default ?? throw new NullReferenceException("team icon is null");
                break;
            }
        }
    }
    
    // public Branch(JSONBranchInChapters jsonBranchInChapters, int index) {
    //     Index = index;
    //     Id = jsonBranchInChapters.BranchId ?? throw new NullReferenceException("branch id is null");
    //     Id = jsonBranchInChapters.JSONTeams[0]. ?? throw new NullReferenceException("branch id is null");
    // }

    public override string ToString() => $"""
                                          
                                          Id        {Id}
                                          TeamName  {TeamName}
                                          TeamIcon  {TeamIcon}
                                          Chapters: {PrintChapters()}
                                          """;

    string PrintChapters() {
        StringBuilder sb = new();
        // foreach (Chapter chapter in Chapters) { // TODO вернуть
        // sb.Append(chapter);
        // }
        for (int index = 0; index < 5; index++) {
            Chapter chapter = Chapters[index];
            sb.Append('\n').Append(chapter);
        }

        sb.Replace("\n", "\n    ");
        return sb.ToString();
    }

}