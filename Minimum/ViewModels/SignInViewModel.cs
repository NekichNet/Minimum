using Microsoft.Extensions.DependencyInjection;
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
    public class SignInViewModel : ViewModelBase
    {
        public event Action<string>? Authenticated;
        private readonly SignInUpView _signInUpView;

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

        public SignInViewModel(SignInUpView signInUpView)
        {
            _ = CheckToken();
            _signInUpView = signInUpView;
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
            _signInUpView.ShowSignUpView();
            await Task.CompletedTask;
        }


        private async Task TrySignIn()
        {
            try
            {
                var scm = App.ServiceProvider.GetRequiredService<ServerConnectionManager>();
                var resp = await scm.SignIn(Input_Login, Input_Password);

                if (resp != null && resp.Success)
                {
                    if (!string.IsNullOrWhiteSpace(resp.Token))
                    {
                        Authenticated?.Invoke(resp.Token);

                        await AutoLogin();
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

        private async Task CheckToken()
        {
            var token = await App.ServiceProvider.GetRequiredService<CacheService>().LoadTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                var con = App.ServiceProvider.GetRequiredService<ServerConnectionManager>();
                var res = await con.CheckToken(token);

                if (res.Success == true)
                {
                    await AutoLogin();
                }
            }
        }

        private async Task AutoLogin()
        {
            var win = new MainWindow();
            win.Show();

            _signInUpView.Close();
        }
    }
}
