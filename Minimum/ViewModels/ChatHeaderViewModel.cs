using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatHeaderViewModel : ViewModelBase
    {
        public string HeaderTitle { get; set; } = "Template title";
        public string HeaderImageSource { get; set; } = string.Empty;
        public string BTN_CONTENT { get; set; } = "Вверх/вниз";




        public string Text_DeleteChatButton { get; set; } = "Удалить чат";
        public string Text_QuitChatButton { get; set; } = "Покинуть чат";
        public string Text_ChangeChatBGButton { get; set; } = "Изменить фон чата";



    }
}
