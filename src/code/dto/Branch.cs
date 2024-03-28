using System.Collections.Generic; // TODO 

namespace libme_scrapper.code.dto;

class Branch(int branchId, string teamName, string teamIcon) {
    public int BranchId { get; set; } = branchId;
    public string TeamName { get; set; } = teamName; // TODO set -> init? во всех свойствах
    public string TeamIcon { get; set; } = teamIcon; // TODO set -> init? во всех свойствах
    // public IList<Team> Teams { get; set; } // TODO или одна активная команда?
    
    
    public override string ToString() => $"branchId = {BranchId}\n"; // TODO 
}