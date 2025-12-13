using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.CUI
{
    public class ConsoleUI(string pageTitle, List<MenuOption> options, string pageDescription = "")
    {
        /// <summary>
        /// Заголовок страницы 
        /// </summary>
        public string PageTitle {  get; set; } = pageTitle;

        /// <summary>
        /// Описание странищки под названием 
        /// </summary>
        public string PageDescription { get; set; } = pageDescription;

        /// <summary>
        /// Курсор, который отображается слева от кнопок 
        /// </summary>
        public string CursorStyle { get; set; } = " > ";

        /// <summary>
        /// Позиция курсора по вертикали внутри списка 
        /// (учитываются все элементы, по этому, например, промеж двух кнопок будет находиться BLANK_SPACE, то он тоже будет учтен.) 
        /// </summary>
        private int CursorPosition = 0;

        /// <summary>
        /// Сообщение, которое будет выводиться КРАСНЫМ цветом 
        /// </summary>
        public string WarningMessage { get; set; } = string.Empty;

        /// <summary>
        /// Сообщение, которое будет выводиться БЕЛЫМ цветом 
        /// </summary>
        public string StatusMessage { get; set; } = string.Empty;

        /// <summary>
        /// Позиции меню 
        /// </summary>
        public List<MenuOption> menuOptions { get; set; } = options;

        /// <summary>
        /// Общее кол-во позиций меню
        /// </summary>
        public int size { get { return menuOptions.Count; } }

        /// <summary>
        /// Метод, который стартует интерфейс
        /// </summary>
        public async Task Start()
        {
            Console.Clear();
            await PushCursorDown();
            while (true) {
                await DrawInterface();
                ConsoleKeyInfo action = Console.ReadKey();

                if (action.Key == ConsoleKey.UpArrow)
                {
                    await PushCursorUp();
                }

                else if (action.Key == ConsoleKey.DownArrow)
                {
                    await PushCursorDown();
                }

                else if (action.Key == ConsoleKey.LeftArrow)
                {
                    await menuOptions[CursorPosition].PushLeft();
                }

                else if (action.Key == ConsoleKey.RightArrow)
                {
                    await menuOptions[CursorPosition].PushRight();
                }

                else if (action.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        await menuOptions[CursorPosition].InvokeAction();
                    }catch(ExceptionUISequenceFinished ex)
                    {
                        _ = ex;
                        break;
                    }
                    catch (Exception ex) { 
                        WarningMessage = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Метод, который пытается опустить курсор ниже
        /// </summary>
        private Task PushCursorDown()
        {
            int midPosition = CursorPosition;
            bool isValidPosition = false;

            while (!isValidPosition)
            {
                midPosition++;
                if (midPosition >= size) {
                    midPosition = CursorPosition;
                    isValidPosition = true;

                    break;
                }
                else if (menuOptions[midPosition].CurrentOptionType == OptionType.BUTTON || menuOptions[midPosition].CurrentOptionType == OptionType.VARIABLE_PROPERTY) {
                    isValidPosition = true;
                    break;
                }
            }
            CursorPosition = midPosition;
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод, который пытается поднять курсор выше
        /// </summary>
        private Task PushCursorUp()
        {
            int midPosition = CursorPosition;
            bool isValidPosition = false;

            while (!isValidPosition)
            {
                midPosition--;
                if (midPosition < 0)
                {
                    midPosition = CursorPosition;
                    isValidPosition = true;

                    break;
                }
                else if (menuOptions[midPosition].CurrentOptionType == OptionType.BUTTON || menuOptions[midPosition].CurrentOptionType == OptionType.VARIABLE_PROPERTY)
                {
                    isValidPosition = true;
                    break;
                }
            }
            CursorPosition = midPosition;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Устонавливает значение поля StatusMessage
        /// </summary>
        public Task SetServerStatus(string msg)
        {
            StatusMessage = msg;
            DrawInterface();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Очищает поле WarningMessage
        /// </summary>
        public Task CleanWarnings()
        {
            WarningMessage = string.Empty;
            DrawInterface();

            return Task.CompletedTask;
        }




        /// <summary>
        /// Рендерит интерфейс в консоль
        /// </summary>
        public Task DrawInterface()
        {
            Console.Clear();
            Console.CursorVisible = false;
            if (!string.IsNullOrEmpty(PageTitle))
            {
                Console.Write($"<<< {PageTitle} >>>\n");
            }
            if (!string.IsNullOrEmpty(PageDescription))
            {
                Console.Write($"-= {PageDescription} =-\n");
            }
            for (int i = 0; i < menuOptions.Count; i++) {

                Console.ForegroundColor = ConsoleColor.White;
                if (CursorPosition == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(CursorStyle);
                }
                else
                {
                    for (int j = 0; j < CursorStyle.Length; j++)
                    {
                        Console.Write(" ");
                    }
                }
                if (menuOptions[i].CurrentOptionType == OptionType.BLANK_SPACE)
                {

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                else if (menuOptions[i].CurrentOptionType == OptionType.HEADER)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"--> {menuOptions[i].Name} <--");
                }
                else
                {

                    Console.Write($"\t {menuOptions[i].Name}");
                    if (menuOptions[i].CurrentOptionType == OptionType.VARIABLE_PROPERTY)
                    {
                        Console.Write($"{menuOptions[i].CurrentPlace}");
                    }

                    Console.Write($"\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            
            if (!string.IsNullOrEmpty(menuOptions[CursorPosition].Description))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\nОписание: {menuOptions[CursorPosition].Description}");

                Console.ForegroundColor = ConsoleColor.White;
            }
            if (!string.IsNullOrEmpty(StatusMessage))
            {

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"\nСтатус сервера: {StatusMessage}\n");
                Console.ForegroundColor = ConsoleColor.White;

            }
            if (!string.IsNullOrEmpty(WarningMessage))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\nВНИМАНИЕ: {WarningMessage}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return Task.CompletedTask;
        }
    }
}
