using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Services;
using Minimum.ViewModels;
using Minimum.Views;
using Color = Avalonia.Media.Color;
using System;

namespace Minimum;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    public static IServiceProvider? ServiceProvider { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ServerConnectionManager>();
        services.AddSingleton<UserProviderService>();

        _serviceProvider = services.BuildServiceProvider();
        ServiceProvider = _serviceProvider;


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new SignInUpView();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}