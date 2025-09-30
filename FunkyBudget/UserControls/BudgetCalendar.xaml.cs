using FunkyBudget.Models;
using FunkyBudget.ViewModels;
using FunkyBudget.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FunkyBudget.UserControls;

public partial class BudgetCalendar : UserControl
{
    #region Routed Commands
    #region AddLineItem Command
    private static readonly RoutedCommand addLineItemCommand = new();
    public static RoutedCommand AddLineItemCommand = addLineItemCommand;
    private void CanExecuteAddLineItemCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is BudgetCalendarViewModel;
    private async void ExecutedAddLineItemCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            Window parent = Window.GetWindow(this);
            Window parentWindow = Application.Current.MainWindow;
            parentWindow.Opacity = 0.5;
            AddLineItemWindow add = new() { Owner = parentWindow };

            if (add.ShowDialog() == true)
            {
                vm.DateTime = DateTime.Now;

                await vm.PopulateLineItems(default);
                await vm.PopulateAvailableBills(default);

                PopulateCalendarGrid();
                PopulateMonthlyLists();
            }

            parentWindow.Opacity = 1.0;
        }
    }
    #endregion

    #region NextMonth Command
    private static readonly RoutedCommand nextMonthCommand = new();
    public static RoutedCommand NextMonthCommand = nextMonthCommand;
    private void CanExecuteNextMonthCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is BudgetCalendarViewModel;
    private void ExecutedNextMonthCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            vm.DateTime = vm.DateTime.AddMonths(1);

            PopulateCalendarGrid();
            PopulateMonthlyLists();
        }
    }
    #endregion

    #region PreviousMonth Command
    private static readonly RoutedCommand previousMonthCommand = new();
    public static RoutedCommand PreviousMonthCommand = previousMonthCommand;
    private void CanExecutePreviousMonthCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is BudgetCalendarViewModel;
    private async void ExecutedPreviousMonthCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            vm.DateTime = vm.DateTime.AddMonths(-1);

            PopulateCalendarGrid();
            PopulateMonthlyLists();
        }
    }
    #endregion
    #endregion

    public BudgetCalendar()
    {
        InitializeComponent();
    }

    #region Events
    private void OnLoad(object sender, RoutedEventArgs e)
    {
        LoadData(default);
    }
    #endregion

    #region Private Functions
    private async Task LoadData(CancellationToken cancellationToken = default)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            vm.DateTime = DateTime.Now;

            await vm.PopulateLineItems(default);
            await vm.PopulateAvailableBills(default);

            PopulateCalendarGrid();
            PopulateMonthlyLists();
        }
    }

    private void PopulateCalendarGrid()
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            grdCalendar.Children.Clear();
            grdCalendar.RowDefinitions.Clear();

            int firstDay = (int)new DateTime(vm.DateTime.Year, vm.DateTime.Month, 1).DayOfWeek;
            if (firstDay > 1)
                PopulateCalendarBlankBeginning(firstDay);

            var days = DateTime.DaysInMonth(vm.DateTime.Year, vm.DateTime.Month);
            for (int i = 0; i < days; i++)
            {
                DateTime current = new(vm.DateTime.Year, vm.DateTime.Month, i + 1);
                var day = (int)current.DayOfWeek;
                var week = current.WeekOfMonth();

                if (grdCalendar.RowDefinitions.Count < week)
                    grdCalendar.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(125, GridUnitType.Pixel) });

                Border border = new()
                {
                    Background = (SolidColorBrush)Application.Current.Resources["Darker"],
                    BorderBrush = (SolidColorBrush)Application.Current.Resources["Lighter"],
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0),
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                CalendarDay calendarDay = new([.. vm.LineItems], current);
                border.Child = calendarDay;
                Grid.SetColumn(border, day);
                Grid.SetRow(border, week - 1);
                grdCalendar.Children.Add(border);
            }

            int lastDay = (int)new DateTime(vm.DateTime.Year, vm.DateTime.Month, days).DayOfWeek;
            int lastWeek = new DateTime(vm.DateTime.Year, vm.DateTime.Month, days).WeekOfMonth();
            if (lastDay < 6)
                PopulateCalendarBlankEnding(lastDay, lastWeek);
        }
    }

    private void PopulateCalendarBlankBeginning(int firstDay)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            grdCalendar.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(125, GridUnitType.Pixel) });
            Border border = new()
            {
                Background = (SolidColorBrush)Application.Current.Resources["Lighter"],
                BorderBrush = (SolidColorBrush)Application.Current.Resources["Lighter"],
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Grid.SetColumn(border, 0);
            Grid.SetColumnSpan(border, firstDay);
            Grid.SetRow(border, 0);
            grdCalendar.Children.Add(border);
        }
    }

    private void PopulateCalendarBlankEnding(int lastDay, int lastWeek)
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            Border border = new()
            {
                Background = (SolidColorBrush)Application.Current.Resources["Lighter"],
                BorderBrush = (SolidColorBrush)Application.Current.Resources["Lighter"],
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Grid.SetColumn(border, lastDay + 1);
            Grid.SetColumnSpan(border, 7 - lastDay - 1);
            Grid.SetRow(border, lastWeek - 1);
            grdCalendar.Children.Add(border);
        }
    }

    private void PopulateMonthlyLists()
    {
        if (DataContext is BudgetCalendarViewModel vm)
        {
            List<LineItem> lineItems = [.. vm.LineItems.Where(w => w.StartDate <= vm.DateTime && (w.EndDate is null || w.EndDate >= vm.DateTime))];
            foreach (LineItem lineItem in lineItems)
            {
                foreach (Bill bill in lineItem.Bills)
                {
                    if (bill.DueDate.Month != vm.DateTime.Month || bill.DueDate.Year != vm.DateTime.Year || lineItem.IsPaidOnCC is null)
                        continue;

                    //add to Checking
                    BudgetLineItem budgetLineItem = new()
                    {
                        AmountDue = lineItem.Amount,
                        BillId = bill.Id,
                        IsPaid = bill.IsPaid,
                        PayTo = lineItem.Name
                    };
                    DockPanel.SetDock(budgetLineItem, Dock.Top);
                    if (lineItem.IsPaidOnCC == true)
                        dpCreditCard.Children.Add(budgetLineItem);
                    else if (lineItem.IsPaidOnCC == false)
                        dpChecking.Children.Add(budgetLineItem);
                }
            }
        }
    }
    #endregion
}
