using Avalonia;
using Avalonia.Data.Converters;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Converters
{
    internal class IdToBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                object accent1, accent2;
                Application.Current.Resources.TryGetResource("Accent1", Application.Current.ActualThemeVariant, out accent1);
                Application.Current.Resources.TryGetResource("Accent2", Application.Current.ActualThemeVariant, out accent2);
                return (value as User).Id != App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Id ? accent1.ToString() : accent2.ToString();
            }
            return "Red";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
