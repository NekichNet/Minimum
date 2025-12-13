using Minimum.Services;
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
    public class SignUpViewModel : ViewModelBase
    {
        public string Input_Login { get; set; } = string.Empty;
        public string Text_LoginWatermark { get; set; } = "Введите логин";

        [Reactive] public char Text_PasswordChar { get; set; } = '•';
        public string Text_PasswordWatermark { get; set; } = "Введите пароль";
        public string Input_Password { get; set; } = string.Empty;
        public string Text_RepeatPasswordWatermark { get; set; } = "Повторите пароль";
        public string Input_RepeatPassword { get; set; } = string.Empty;
        public string Text_ButtonSignUp { get; set; } = "Зарегистрироваться";
        public string Text_ButtonSwitchToSignIn { get; set; } = "Войти";

        public ReactiveCommand<Unit, Unit> Click_SignUp { get; }
        public ReactiveCommand<Unit, Unit> Click_GoToSignIn { get; }




        public SignUpViewModel()
        {
            Click_SignUp = ReactiveCommand.CreateFromTask(TrySignUp);
            Click_GoToSignIn = ReactiveCommand.CreateFromTask(GoToSignIn);
        }



        private async Task TrySignUp()
        {
            var req = new ServerConnectionManager();
            await req.StartConnection();
            var res = await req.SignUp(Input_Login, Input_Password);
        }
        private async Task GoToSignIn()
        {
            // любой код для перехода на страницу входа
        }

    }
}
