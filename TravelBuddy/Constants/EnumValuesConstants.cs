using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using TravelBuddy.Models;
using TravelBuddy.Models.Enums;
using TravelBuddy.Utils;

namespace TravelBuddy.Constants;

public class EnumValuesConstants
{
    public static readonly ObservableCollection<Activity> Activities =
        Enum.GetValues<Activity>().ToObservableCollection();
    
    public static readonly ObservableCollection<string> BudgetRange =
        Enum.GetValues<Budget>().Select(e => e.GetDescription()).ToObservableCollection();
    
    public static readonly ObservableCollection<string> NumberOfCompanions =
        Enum.GetValues<GroupSize>().Select(e => e.GetDescription()).ToObservableCollection();
}