using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Services;
using Minimum.ViewModels;
using Minimum.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Minimum;

public partial class SignInUpView : Window
{
    private readonly CacheService _cacheService;

    public SignInUpView()
    {
        InitializeComponent();
        ShowSignInView();
        _cacheService = App.ServiceProvider.GetRequiredService<CacheService>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // Показать экран входа
    public void ShowSignInView()
    {
        var view = new SignInView(this);
        GetContentHost().Content = view;
        SubscribeAuth(view);
    }

    // Показать экран регистрации
    public void ShowSignUpView()
    {
        var view = new SignUpView(this);
        GetContentHost().Content = view;
        SubscribeAuth(view);
    }

    public async Task LoadInitialViewAsync(Func<Task<bool>> needSignUpProvider)
    {
        if (needSignUpProvider == null) throw new ArgumentNullException(nameof(needSignUpProvider));
        bool needSignUp = await needSignUpProvider();
        if (needSignUp) ShowSignUpView(); else ShowSignInView();
    }

    private ContentControl GetContentHost()
    {
        return this.FindControl<ContentControl>("ContentHost")
            ?? throw new InvalidOperationException("ContentHost не найден в разметке");
    }


    private void SubscribeAuth(UserControl view)
    {
        if (view.DataContext is SignInViewModel signInVm)
        {
            signInVm.Authenticated += token => OnAuthenticated(token);
        }
        else if (view.DataContext is SignUpViewModel signUpVm)
        {
            signUpVm.Authenticated += token => OnAuthenticated(token);
        }
    }


    private void OnAuthenticated(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _ = _cacheService.SaveTokenAsync(token);
        }

        Dispatcher.UIThread.Post(async () =>
        {
            var mainWindow = new MainWindow();

            mainWindow.Show();
            await Task.Delay(1); 
            this.Close();
        });
    }


    public async Task LoadCachedChatsAsync()
    {
        var cachedChats = await _cacheService.LoadChatsAsync();
        foreach (var chat in cachedChats)
        {
            /*
            Chats.Add(new ChatViewModel(chat, _cacheService));
            */
        }
    }
}