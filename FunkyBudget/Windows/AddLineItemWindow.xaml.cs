using FunkyBudget.Core.Extensions;
using FunkyBudget.Models.Enums;
using FunkyBudget.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FunkyBudget.Windows;

public partial class AddLineItemWindow : Window
{
    #region Routed Commands
    #region AddLineItem Command
    private static readonly RoutedCommand addLineItemCommand = new();
    public static RoutedCommand AddLineItemCommand = addLineItemCommand;
    private void CanExecuteAddLineItemCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is AddLineItemViewModel vm && vm.IsDataValid();
    private async void ExecutedAddLineItemCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is AddLineItemViewModel vm)
            DialogResult = await vm.AddLineItemAsync(default);

        Close();
    }
    #endregion

    #region Cancel Command
    private static readonly RoutedCommand cancelCommand = new();
    public static RoutedCommand CancelCommand = cancelCommand;
    private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    #endregion
    #endregion

    public AddLineItemWindow()
    {
        InitializeComponent();
    }

    #region Events
    private void OnDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void OnFrequencyChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is AddLineItemViewModel vm)
        {
            if (sender is ToggleButton tbSender && tbSender.Content is TextBlock txSender)
            {
                if (tbSender.IsChecked == false)
                    tbSender.IsChecked = true;

                foreach (var item in wpFrequencies.Children)
                    if (item is ToggleButton tb && tb is not null)
                        tb.IsChecked = tb.Content is TextBlock tx && tx is not null && tx.Text == txSender.Text;

                vm.Frequency = EnumExtensions.GetEnumValueFromDescription<Frequency>(txSender.Text);
            }
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        foreach (string frequency in Enum.GetValues<Frequency>().Cast<Frequency>().Select(s => s.GetDescription()).ToList())
        {
            ToggleButton tb = new()
            {
                Content = new TextBlock()
                {
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Text = frequency
                },
                Height = 20,
                Style = (Style)FindResource("FrequencyToggleStyle")
            };
            tb.Click += OnFrequencyChanged;

            wpFrequencies.Children.Add(tb);
        }
    }
    #endregion
}
