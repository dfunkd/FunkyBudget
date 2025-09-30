using FunkyBudget.Services;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace FunkyBudget.Windows;

public partial class BudgetWindow : Window
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

    public BudgetWindow()
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

    #region Full Screen Prevent Covering Taskbar
    //https://stackoverflow.com/questions/20941443/properly-maximizing-wpf-window-with-windowstyle-none
    private void OnSourceInitialized(object sender, EventArgs e)
    {
        IntPtr mWindowHandle = (new WindowInteropHelper(this)).Handle;
        HwndSource.FromHwnd(mWindowHandle).AddHook(new HwndSourceHook(WindowProc));
    }

    private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case 0x0024:
                WmGetMinMaxInfo(hwnd, lParam);
                break;
        }

        return IntPtr.Zero;
    }

    private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
    {
        GetCursorPos(out POINT lMousePosition);

        IntPtr lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
        MONITORINFO lPrimaryScreenInfo = new MONITORINFO();
        if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            return;

        IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

        MINMAXINFO lMmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

        if (lPrimaryScreen.Equals(lCurrentScreen) == true)
        {
            lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
            lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
            lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
            lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
        }
        else
        {
            lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
            lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
            lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
            lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
        }

        Marshal.StructureToPtr(lMmi, lParam, true);
    }
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    [DllImport("user32.dll")]
    static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT(int x, int y)
    {
        public int X = x;
        public int Y = y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf<MONITORINFO>();
        public RECT rcMonitor = new();
        public RECT rcWork = new();
        public int dwFlags = 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT(int left, int top, int right, int bottom)
    {
        public int Left = left, Top = top, Right = right, Bottom = bottom;
    }
    #endregion
}
