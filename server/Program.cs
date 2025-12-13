
using server.CUI;

namespace server;
internal class Program
{
    static void Main(string[] args)
    {

        ConsoleUI Page1UI = new ConsoleUI(
            "Вспомогательная страница",
            new List<MenuOption>
            {
                new MenuOption{Name = "Вау! Кнопка!", CurrentOptionType = OptionType.BUTTON_RETURN}
            }
            );


        ConsoleUI MainUI = new ConsoleUI(
            "Главная страница",
            new List<MenuOption>
            {
                new MenuOption{Name = "Открыть вспомогательное меню", Action = Page1UI.Start}
            }
            );
        _ = MainUI.Start();
    }
}
