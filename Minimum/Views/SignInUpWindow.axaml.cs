using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;
using System;
using System.Threading.Tasks;

namespace Minimum;

public partial class SignInUpView : Window
{
    public SignInUpView()
    {
        InitializeComponent();
    }


    public SignInUpView(bool startWithSignUp) : this()
    {
        if (startWithSignUp)
            ShowSignUpView();
        else
            ShowSignInView();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // Показать экран входа
    public void ShowSignInView()
    {
        var view = new SignInView();
        GetContentHost().Content = view;
    }

    // Показать экран регистрации
    public void ShowSignUpView()
    {
        var view = new SignUpView();
        GetContentHost().Content = view;
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
}