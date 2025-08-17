using System.ComponentModel;

namespace TravelBuddy.Models.Enums;

public enum Budget
{
    [Description("Not specified")]
    Undefined,

    [Description("Economy – Basic, low-cost options")]
    Economy,

    [Description("Budget – Affordable, good value")]
    Budget,

    [Description("Mid-Range – Balanced comfort and price")]
    MidRange,

    [Description("Premium – Higher-end experience")]
    Premium,

    [Description("Luxury – Top-tier, exclusive options")]
    Luxury
}