using System.ComponentModel;

namespace TravelBuddy.Models;

public enum GroupSize
{
    [Description("Not specified")]
    Undefined,
    
    [Description("Solo")]
    Solo,

    [Description("Team of two")]
    Couple,

    [Description("Small group (3–5)")]
    SmallGroup,

    [Description("Medium group (6–10)")]
    MediumGroup,

    [Description("Large group (10+)")]
    LargeGroup
}