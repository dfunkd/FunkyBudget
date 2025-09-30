using FunkyBudget.Models;
using FunkyBudget.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FunkyBudget.UserControls;

public partial class CalendarDay : UserControl
{
    #region Properties
    private SolidColorBrush lightest { get; set; }
    private SolidColorBrush paid { get; set; }
    private SolidColorBrush unpaid { get; set; }
    private SolidColorBrush unpaidDueSoon { get; set; }
    private List<LineItem> lineItems { get; set; }
    #endregion

    #region Routed Commands
    #region AddItem Command
    private static readonly RoutedCommand addItemCommand = new();
    public static RoutedCommand AddItemCommand = addItemCommand;
    private void CanExecuteAddItemCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedAddItemCommand(object sender, ExecutedRoutedEventArgs e)
    {
        AddBill();
    }
    #endregion
    #endregion

    public CalendarDay(List<LineItem> _lineItems, DateTime dueDate)
    {
        InitializeComponent();

        lightest = (SolidColorBrush)Application.Current.Resources["Lightest"];
        paid = (SolidColorBrush)Application.Current.Resources["Paid"];
        unpaid = (SolidColorBrush)Application.Current.Resources["Unpaid"];
        unpaidDueSoon = (SolidColorBrush)Application.Current.Resources["UnpaidDue"];

        lineItems = _lineItems;

        if (DataContext is CalendarDayViewModel vm)
            vm.DueDate = dueDate;
    }

    #region Methods
    private void OnLoad(object sender, RoutedEventArgs e)
    {
        if (DataContext is not CalendarDayViewModel vm)
            return;

        dpBills.Children.Clear();

        foreach (var item in lineItems)
        {
            foreach (var b in item.Bills)
            {
                if (b.DueDate.Day != vm.DueDate.Day || b.DueDate.Month != vm.DueDate.Month || b.DueDate.Year != vm.DueDate.Year)
                    continue;

                DockPanel bill = new() { HorizontalAlignment = HorizontalAlignment.Right, LastChildFill = false };
                DockPanel.SetDock(bill, Dock.Top);
                CheckBox cbIsPaid = new()
                {
                    IsChecked = b.IsPaid
                };
                TextBlock biller = new()
                {
                    Foreground = lightest,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(5, 0, 5, 0),
                    Text = item.Name
                };
                TextBlock amount = new()
                {
                    Foreground = item.IsCredit == true ? paid : item.DueDate <= DateTime.Now.Day ? unpaidDueSoon : unpaid,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(5, 0, 5, 0),
                    Text = $"${item.Amount}"
                };
                DockPanel.SetDock(biller, Dock.Left);
                DockPanel.SetDock(amount, Dock.Left);
                bill.Children.Add(biller);
                bill.Children.Add(amount);
                dpBills.Children.Add(bill);
            }
        }
        /*
        DockPanel bill1 = new() { HorizontalAlignment = HorizontalAlignment.Right, LastChildFill = false };
        DockPanel.SetDock(bill1, Dock.Top);
        TextBlock biller1 = new()
        {
            Foreground = lightest,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "MidAmerican"
        };
        TextBlock amount1 = new()
        {
            Foreground = unpaid,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "$94.56"
        };
        DockPanel.SetDock(biller1, Dock.Left);
        DockPanel.SetDock(amount1, Dock.Left);
        bill1.Children.Add(biller1);
        bill1.Children.Add(amount1);
        dpBills.Children.Add(bill1);

        DockPanel bill2 = new() { HorizontalAlignment = HorizontalAlignment.Right, LastChildFill = false };
        DockPanel.SetDock(bill2, Dock.Top);
        TextBlock biller2 = new()
        {
            Foreground = lightest,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "Car Payment"
        };
        TextBlock amount2 = new()
        {
            Foreground = unpaidDueSoon,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "$675.00"
        };
        DockPanel.SetDock(biller2, Dock.Left);
        DockPanel.SetDock(amount2, Dock.Left);
        bill2.Children.Add(biller2);
        bill2.Children.Add(amount2);
        dpBills.Children.Add(bill2);

        DockPanel bill3 = new() { HorizontalAlignment = HorizontalAlignment.Right, LastChildFill = false };
        DockPanel.SetDock(bill3, Dock.Top);
        TextBlock biller3 = new()
        {
            Foreground = lightest,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "Mortgage"
        };
        TextBlock amount3 = new()
        {
            Foreground = paid,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "$933.42"
        };
        DockPanel.SetDock(biller3, Dock.Left);
        DockPanel.SetDock(amount3, Dock.Left);
        bill3.Children.Add(biller3);
        bill3.Children.Add(amount3);
        dpBills.Children.Add(bill3);
        */
    }

    private void AddBill()
    {
        DockPanel bill1 = new() { HorizontalAlignment = HorizontalAlignment.Right, LastChildFill = false };
        DockPanel.SetDock(bill1, Dock.Top);
        TextBlock biller1 = new()
        {
            Foreground = (SolidColorBrush)Application.Current.Resources["Lightest"],
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "MidAmerican"
        };
        TextBlock amount1 = new()
        {
            Foreground = (SolidColorBrush)Application.Current.Resources["Unpaid"],
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 5, 0),
            Text = "$94.56"
        };
        DockPanel.SetDock(biller1, Dock.Left);
        DockPanel.SetDock(amount1, Dock.Left);
        bill1.Children.Add(biller1);
        bill1.Children.Add(amount1);
        dpBills.Children.Add(bill1);
    }
    #endregion
}
