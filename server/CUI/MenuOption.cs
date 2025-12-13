using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.CUI
{
    public class MenuOption 
    {

        /// <summary>
        /// Название опции, которое будет отображено в меню. (Если CurrentOptionType стоит значение OptionType.BLANK_SPACE, то название будет проигнорировано)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание опции, которое будет отображено в меню при выборе опции курсором. (Если CurrentOptionType стоит значение OptionType.BLANK_SPACE, то описание будет проигнорировано)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Делегат присваиваемого метода
        /// </summary>
        public delegate Task MenuOptionDelegate();

        /// <summary>
        /// Переменная делегата присваиваемого метода
        /// </summary>
        public MenuOptionDelegate Action { get; set; }

        /// <summary>
        /// Минимальный лимит при установке CurrentOptionType в значение OptionType.VARIABLE_PROPERTY
        /// </summary>
        public int MinLimit {  get; set; } = 0;
        /// <summary>
        /// Максимальный лимит при установке CurrentOptionType в значение OptionType.VARIABLE_PROPERTY
        /// </summary>
        public int MaxLimit { get; set; } = 100;

        /// <summary>
        /// Текущая позиция при CurrentOptionType в значение OptionType.VARIABLE_PROPERTY, изменяется по средством нажатия стрелок в бок.
        /// </summary>
        public int CurrentPlace { get; set; } = 0;

        /// <summary>
        /// Текущий OptionType, чтобы интерфейс мог понять, как взаимодействовать с этой опцией
        /// </summary>
        public OptionType CurrentOptionType { get; set; }



        public MenuOption(string name = "[blank]", MenuOptionDelegate method = null, OptionType optionType = OptionType.BUTTON, string description = "")
        {
            Name = name;
            Description = description;
            Action = method;
            CurrentOptionType = optionType;
        }



        /// <summary>
        /// Запускает метод присвоеный свойству Action
        /// </summary>
        public Task InvokeAction() {
            if (CurrentOptionType == OptionType.BUTTON_RETURN)
            {
                throw new ExceptionUISequenceFinished();
            }

            else if (Action != null)
            {
                Action.Invoke();
            }
            else
            {
                if (CurrentOptionType == OptionType.VARIABLE_PROPERTY)
                {
                    throw new InvalidOperationException($"Действие для данной опции (Name: {Name}) меню не присвоено,\nданная опция отвечает за ось, попробуйте использовать стрелочки.");

                }
                else
                {
                    throw new InvalidOperationException($"Действие для данной опции (Name: {Name}) меню не присвоено");
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Пытается увеличить значение CurrentPlace
        /// </summary>
        public Task PushRight()
        {
            if (CurrentPlace+1 <= MaxLimit)
            {
                CurrentPlace++;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Пытается уменьшить значение CurrentPlace
        /// </summary>
        public Task PushLeft()
        {
            if (CurrentPlace - 1 >= MinLimit)
            {
                CurrentPlace--;
            }
            return Task.CompletedTask;
        }

    }
}
