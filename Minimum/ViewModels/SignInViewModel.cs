using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class SignInViewModel
    {
        public string Input_Login { get; set; } = string.Empty;
        public string Text_LoginWatermark { get; set; } = "Введите логин";

        public string Input_Password { get; set; } = string.Empty;
        [Reactive] public char Text_PasswordChar { get; set; }
        [Reactive] public bool Bool_RevealPassword { get; set; }
        public string Text_PasswordWatermark { get; set; } = "Введите пароль";
        public string Text_ButtonSignIn { get; set; } = "Войти";
        public string Text_ButtonSwitchToSignUp { get; set; } = "Зарегистрироваться";
        public ReactiveCommand<Unit, Unit> Click_RevealPassword { get; }

        public SignInViewModel()
        {
            Bool_RevealPassword = false;
            Text_PasswordChar = '•';
            Click_RevealPassword = ReactiveCommand.Create(RevealPassword);
        }



        private void RevealPassword()
        {
            if (Text_PasswordChar == '•')
            {
                Text_PasswordChar = '\0';
                Bool_RevealPassword = true;
            }
            else
            {
                Text_PasswordChar = '•';

                Bool_RevealPassword = false;
            }
        }

    }
}
