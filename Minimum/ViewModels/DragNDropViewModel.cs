using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class DragNDropViewModel : ReactiveObject
    {
        public delegate void FilesWereDroppedDelegate(string[] droppedFilesPaths);
        public FilesWereDroppedDelegate? FilesWereDropped { get; set; }

        private string _textClue = "Перетащите сюда файл";
        public string Text_DragNDropClue
        {
            get => _textClue;
            set => this.RaiseAndSetIfChanged(ref _textClue, value);
        }

        private string _backgroundColor = "Blue";
        public string BackgroundColor
        {
            get => _backgroundColor;
            set => this.RaiseAndSetIfChanged(ref _backgroundColor, value);
        }

        public void HandleFilesDropped(string[] filePaths)
        {
            FilesWereDropped?.Invoke(filePaths);
            Console.WriteLine("Dropped files: " + string.Join(", ", filePaths));
        }
    }
}
