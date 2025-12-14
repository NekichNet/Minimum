using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;
using System.Linq;

namespace Minimum;

public partial class DragNDropView : UserControl
{
    private DragNDropViewModel _vm;

    public DragNDropView()
    {
        InitializeComponent();
        _vm = new DragNDropViewModel();
        DataContext = _vm;
    }

    private void Grid_DragEnter(object? sender, Avalonia.Input.DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            _vm.Text_DragNDropClue = "Отпустите файл здесь";
            _vm.BackgroundColor = "LightGreen";
        }
    }

    private void Grid_DragLeave(object? sender, Avalonia.Input.DragEventArgs e)
    {
        _vm.Text_DragNDropClue = "Перетащите сюда файл";
        _vm.BackgroundColor = "Blue";
    }


    private void Grid_Drop(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            var storageItems = e.Data.GetFiles();
            var paths = storageItems?
                .Select(si => si?.Path?.ToString())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            if (paths != null && paths.Length > 0)
            {
                _vm.HandleFilesDropped(paths);
                _vm.Text_DragNDropClue = $"Загружено: {paths.Length} файл(ов)";
            }
            else
            {
                _vm.Text_DragNDropClue = "Перетащите сюда файл";
            }

            _vm.BackgroundColor = "Blue";
        }
    }
}