using System.ComponentModel;

namespace FunkyBudget.Models.Enums;

public enum Frequency
{
    [Description("One Time")]
    OneTime = 1,
    [Description("Daily")]
    Daily = 2,
    [Description("Weekly")]
    Weekly = 3,
    [Description("Biweekly")]
    Biweekly = 4,
    [Description("1st and 15th")]
    FirstAndFifteenth = 5,
    [Description("Monthly")]
    Monthly = 6,
    [Description("Yearly")]
    Yearly = 7
}
