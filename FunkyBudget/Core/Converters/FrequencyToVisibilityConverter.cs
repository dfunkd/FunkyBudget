using FunkyBudget.Models.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FunkyBudget.Core.Converters;

public class FrequencyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (Enum.TryParse<Frequency>(value.ToString(), out Frequency frequency))
            return frequency == Frequency.OneTime || frequency == Frequency.Monthly ? Visibility.Visible : Visibility.Collapsed;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
