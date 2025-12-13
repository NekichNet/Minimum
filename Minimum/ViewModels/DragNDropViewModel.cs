using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class DragNDropViewModel
    {
        public delegate void FilesWereDroppedDelegate(string[] droppedFilesPaths);
        public FilesWereDroppedDelegate FilesWereDropped { get; set; }
        public string Text_DragNDropClue { get; set; } = "Перетащите сюда файл";



        public void HandleFilesDropped(string[] filePaths)
        {
            if (FilesWereDropped != null)
            {
                FilesWereDropped.Invoke(filePaths);
            }
        }

    }
}
