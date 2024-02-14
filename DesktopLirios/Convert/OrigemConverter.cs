using System;
using System.Globalization;
using System.Windows.Data;

public class OrigemConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int metodoPagamento)
        {
            switch (metodoPagamento)
            {
                case 1:
                    return "Avon";
                case 2:
                    return "Cacau Show";
                case 3:
                    return "Eudora";
                case 4:
                    return "Natura";
                case 5:
                    return "O Boticario";
                case 6:
                    return "Tupperware";
                default:
                    return "Diversos";
            }
        }

        return "Diversos";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}