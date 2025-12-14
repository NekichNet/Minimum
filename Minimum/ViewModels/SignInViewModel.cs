using Minimum.Services;
using Minimum.Views;
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
        public event Action<string>? Authenticated;

        public string Input_Login { get; set; } = string.Empty;
        public string Text_LoginWatermark { get; set; } = "Введите логин";

        public string Input_Password { get; set; } = string.Empty;
        [Reactive] public char Text_PasswordChar { get; set; }
        [Reactive] public bool Bool_RevealPassword { get; set; }
        public string Text_PasswordWatermark { get; set; } = "Введите пароль";
        public string Text_ButtonSignIn { get; set; } = "Войти";
        public string Text_ButtonSwitchToSignUp { get; set; } = "Зарегистрироваться";
        public ReactiveCommand<Unit, Unit> Click_RevealPassword { get; }
        public ReactiveCommand<Unit, Unit> Click_SignIn { get; }
        public ReactiveCommand<Unit, Unit> Click_GoToSignUp { get; }

        public SignInViewModel()
        {
            Bool_RevealPassword = false;
            Text_PasswordChar = '•';
            Click_RevealPassword = ReactiveCommand.Create(RevealPassword);

            Click_SignIn = ReactiveCommand.CreateFromTask(TrySignIn);
            Click_GoToSignUp = ReactiveCommand.CreateFromTask(GoToSignUp);
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


        private async Task GoToSignUp()
        {
            var nav = new NavigationService();
            nav.NavigateTo<SignUpView>();
            await Task.CompletedTask;
        }


        private async Task TrySignIn()
        {
            try
            {
                var scm = new Services.ServerConnectionManager();
                await scm.StartConnection();
                var resp = await scm.SignIn(Input_Login, Input_Password);

                if (resp != null && resp.Success)
                {
                    if (!string.IsNullOrWhiteSpace(resp.Token))
                    {
                        Authenticated?.Invoke(resp.Token);


                        var nav = new NavigationService();
                        nav.NavigateTo<ChatView>();
                    }
                }
                else
                {
                    // можно показать ошибку пользователю
                }
            }
            catch
            {
                // логика обработки ошибок
            }
        }
    }
}
