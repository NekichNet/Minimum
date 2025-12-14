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
                return (value as User).Id != App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Id ? "Gray" : "Blue";
            }
            return "Red";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
