using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Minimum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class NavigationService : INavigationService, INotifyPropertyChanged
    {
        private readonly Stack<UserControl> _history = new();
        private readonly Stack<UserControl> _navigationStack;
        public bool CanGoBack => _history.Count > 0;
        private UserControl? _current;

        public UserControl CurrentUserControl => _current ?? (GetMainWindow()?.Content as UserControl ?? throw new InvalidOperationException("Нет текущего UserControl"));
        public event PropertyChangedEventHandler? PropertyChanged;



        public void NavigateTo(UserControl view)
        {
            var main = GetMainWindow();
            if (main == null)
            {
                PushCurrentIfExists();
                _current = view;
                OnPropertyChanged(nameof(CurrentUserControl));
                OnPropertyChanged(nameof(CanGoBack));
                return;
            }


            Dispatcher.UIThread.Post(() =>
            {
                PushCurrentIfExists();
                _current = view;
                main.Content = view;
                OnPropertyChanged(nameof(CurrentUserControl));
                OnPropertyChanged(nameof(CanGoBack));
            });
        }



        private void PushCurrentIfExists()
        {
            try
            {
                var existing = GetMainWindow()?.Content as UserControl ?? _current;
                if (existing != null)
                {
                    if (_history.Count == 0 || _history.Peek() != existing)
                        _history.Push(existing);
                }
            }
            catch
            {
            }
        }


        public void GoBack()
        {
            if (_history.Count == 0) return;

            var prev = _history.Pop();
            var main = GetMainWindow();

            Dispatcher.UIThread.Post(() =>
            {
                _current = prev;
                if (main != null)
                    main.Content = prev;

                OnPropertyChanged(nameof(CurrentUserControl));
                OnPropertyChanged(nameof(CanGoBack));
            });
        }


        public void NavigateTo<T>() where T : UserControl
        {
            NavigateTo(typeof(T));
        }


        private void NavigateTo(Type type)
        {
            if (!typeof(UserControl).IsAssignableFrom(type))
                throw new ArgumentException("Type must be a UserControl", nameof(type));

            var control = Activator.CreateInstance(type) as UserControl
                ?? throw new InvalidOperationException($"Не удалось создать экземпляр {type.FullName}");

            NavigateTo(control);
        }


        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }



        private static Window? GetMainWindow()
        {
            return Application.Current?.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
                _ => null
            };
        }
    }
}
