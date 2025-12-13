using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;
using System.Linq;

namespace Minimum;

public partial class DragNDropView : UserControl
{
    public DragNDropView()
    {
        InitializeComponent();
        DataContext = new DragNDropViewModel();
    }

    private void Grid_DragEnter(object? sender, Avalonia.Input.DragEventArgs e)
    {
    }

    private void Grid_DragLeave(object? sender, Avalonia.Input.DragEventArgs e)
    {

    }

    private void Grid_Drop(object? sender, Avalonia.Input.DragEventArgs e)
    {
        //var data = e.DataTransfer.TryGetValues();
    }
}