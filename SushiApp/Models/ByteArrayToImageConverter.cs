using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace SushiApp.Models
{
    /// <summary>
    /// class for convertion byte[] to image to show products
    /// </summary>
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageAsBytes = (byte[])value;
            return ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
