using System;

namespace libme_scrapper.code.dto;

class Chapter {

    public int BranchId { get; set; }
    public int Index { get; set; }
    public string Volume { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }

    public Chapter(JSONChapter jsonChapter, int branchIndex) {
        Index = jsonChapter.Index ?? throw new NullReferenceException("volume is null");
        Volume = jsonChapter.Volume ?? throw new NullReferenceException("volume is null");
        Number = jsonChapter.Number ?? throw new NullReferenceException("number is null");
        Name = jsonChapter.Name ?? throw new NullReferenceException("name is null");
        BranchId = jsonChapter.JSONBranches![branchIndex].BranchId ?? -1;
        // BranchId = jsonChapter.JSONBranches![branchIndex].BranchId ?? throw new NullReferenceException("branch id is null, branchIndex=" + branchIndex);
        Link = BranchId == -1 ? $"/chapter?number={Number}&volume={Volume}" : $"/chapter?branch_id={BranchId}&number={Number}&volume={Volume}";
    }

    // public Chapter(JSONChapter jsonChapter, int branchId) {
    //     Index = jsonChapter?.Index ?? -1;
    //     Volume = jsonChapter?.Volume ?? string.Empty;
    //     Number = jsonChapter?.Number ?? string.Empty;
    //     BranchId = branchId;
    //     Name = jsonChapter?.Name ?? string.Empty;
    //     Link = $"/chapter?branch_id={BranchId}&number={Number}&volume={Volume}"; // TODO если наллы геттеры переделать
    // }

    public override string ToString() => $"""
                                          
                                          BranchId  {BranchId}
                                          Index     {Index}
                                          Volume    {Volume}
                                          Number    {Number}
                                          Name      {Name}
                                          Link      {Link}
                                          """;
}