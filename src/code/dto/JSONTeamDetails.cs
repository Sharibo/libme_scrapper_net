using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace libme_scrapper.code.dto;

class JSONTeamDetails {
    [J("branch_id")] public int? BranchId { get; set; } // TODO по сути не нужно
    [J("is_active")] public bool? IsActive { get; set; }
    // [J("subscriptions_count")] public object SubscriptionsCount { get; set; }

    
    public override string ToString() => $"""

                                          BranchId  {BranchId}
                                          IsActive  {IsActive}
                                          """;
}