using System.Windows.Controls;

namespace FunkyBudget.UserControls;

public partial class BudgetLineItem : UserControl
{
    #region Properties
    public bool IsPaid { get; set; }
    public decimal AmountDue { get; set; }
    public int BillId { get; set; }
    public string PayTo { get; set; }
    #endregion

    public BudgetLineItem()
        => InitializeComponent();

    private void OnPaidChanged(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
