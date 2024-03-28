using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

public class JSONTeam {
    [J("id")]        public int? Id { get; set; }      
    [J("name")]      public string Name { get; set; }   
    [J("slug")]      public string Slug { get; set; }   
    [J("cover")]     public string Cover { get; set; }  
    [J("branch_id")] public int? BranchId { get; set; }
    [J("is_active")] public int? IsActive { get; set; }
    
    public override string ToString() => $"""
                                          
                                            Id        {Id}
                                            Name      {Name}
                                            Slug      {Slug}
                                            Cover     {Cover}
                                            BranchId  {BranchId}
                                            IsActive  {IsActive}
                                            
                                          """;
}