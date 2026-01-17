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
    public class SignUpViewModel : ViewModelBase
    {
        private readonly SignInUpView _signInUpView;
        public event Action<string>? Authenticated;

        private readonly ServerConnectionManager _scm;

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




        public SignUpViewModel(SignInUpView signInUpView)
        {
            _signInUpView = signInUpView;

            _scm = App.ServiceProvider.GetRequiredService<ServerConnectionManager>();

            Click_SignUp = ReactiveCommand.CreateFromTask(TrySignUp);
            Click_GoToSignIn = ReactiveCommand.CreateFromTask(GoToSignIn);
        }



        private async Task TrySignUp()
        {
            //var cacheService = new CacheService();
            //var req = new ServerConnectionManager(cacheService);
            ////await req.StartConnection();
            //var resp = await req.SignUp(Input_Login, Input_Password);

            var resp = await _scm.SignUp(Input_Login, Input_Password);

            if (resp != null && resp.Success)
            {
                if (!string.IsNullOrWhiteSpace(resp.Token) && !string.IsNullOrWhiteSpace(resp.Token))
                {
                    Authenticated?.Invoke(resp.Token);

                    
                }
            }
            else
            {
                // отобразить ошибку/логировать
            }
        }


        private async Task GoToSignIn()
        {
            var nav = new NavigationService();
            _signInUpView.ShowSignInView();
            //nav.NavigateTo<SignInView>();
            await Task.CompletedTask;
        }


        private async Task AutoRegister()
        {
            var win = new MainWindow();
            win.Show();

            _signInUpView.Close();
        }
    }
}
