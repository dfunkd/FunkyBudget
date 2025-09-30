using FunkyBudget.Models.Enums;
using FunkyBudget.Services;
using FunkyBudget.UserControls;
using FunkyBudget.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyBudget;

public partial class MainWindow : Window
{
    #region Private Properties
    private BudgetService service;
    #endregion

    #region RoutedCommands
    #region Close Command
    private static readonly RoutedCommand closeCommand = new();
    public static RoutedCommand CloseCommand = closeCommand;
    private void CanExecuteCloseCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCloseCommand(object sender, ExecutedRoutedEventArgs e)
        => Close();
    #endregion
    #endregion

    public MainWindow()
    {
        InitializeComponent();

        service = new BudgetService();
    }

    #region Events
    private void OnDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private async void OnLoad(object sender, RoutedEventArgs e)
    {
        var test = await service.GetLineItems(default);
        //if (DataContext is BudgetViewModel vm)
        //{
        //    vm.LineItems.Clear();
        //    vm.LineItems.Add(new()
        //    {
        //        Amount = 94.56M,
        //        CreatedBy = "Test",
        //        CreatedDate = DateTime.Now,
        //        DueDate = 28,
        //        EndDate = null,
        //        Id = 1,
        //        IsCredit = false,
        //        IsPaid = false,
        //        ModifiedBy = "Test",
        //        ModifiedDate = DateTime.Now,
        //        Name = "MidAmerican",
        //        PaymentFrequency = Frequency.Monthly,
        //        StartDate = new DateTime(2025, 05, 28)
        //    });
        //    vm.LineItems.Add(new()
        //    {
        //        Amount = 933.42M,
        //        CreatedBy = "Test",
        //        CreatedDate = DateTime.Now,
        //        DueDate = 1,
        //        EndDate = new(2027, 08, 1),
        //        Id = 2,
        //        IsCredit = false,
        //        IsPaid = true,
        //        ModifiedBy = "Test",
        //        ModifiedDate = DateTime.Now,
        //        Name = "Mortgage",
        //        PaymentFrequency = Frequency.Monthly,
        //        StartDate = new DateTime(2025, 05, 1)
        //    });
        //    vm.LineItems.Add(new()
        //    {
        //        Amount = 2646.60M,
        //        CreatedBy = "Test",
        //        CreatedDate = DateTime.Now,
        //        DueDate = 4,
        //        Id = 3,
        //        IsCredit = true,
        //        IsPaid = true,
        //        ModifiedBy = "Test",
        //        ModifiedDate = DateTime.Now,
        //        Name = "Ruan Transportation",
        //        PaymentFrequency = Frequency.Biweekly,
        //        StartDate = new DateTime(2025, 05, 2)
        //    });

        //    vm.DateTime = DateTime.Now;
        //    grdContent.Children.Clear();
        //    grdContent.Children.Add(new BudgetCalendar(vm));
        //}
    }
    #endregion
}
