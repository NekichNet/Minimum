using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.CUI
{
    public enum OptionType
    {
        BUTTON = 0, // Стандартная кнопка, активируется при нажатии Enter
        VARIABLE_PROPERTY = 1, // Кнопка, активируется при нажатии Enter и имеет Counter, с которым можно взаимодействовать стрелочками влево/вправо
        HEADER = 2, // Простой текст. Можно обозначить подкотигорию. Будет проигнорирована курсором.
        BLANK_SPACE = 3, // Пустая строка. Будет проигнорирована курсором.
        BUTTON_RETURN = 4 // Зарезервированая кнопка, которая выйдет из текущего цикла с интерфейсом
    }
}
