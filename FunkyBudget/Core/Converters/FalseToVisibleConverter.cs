using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FunkyBudget.Core.Converters;

public class FalseToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (bool.TryParse(value.ToString(), out bool collapsed))
            return collapsed ? Visibility.Collapsed : Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
