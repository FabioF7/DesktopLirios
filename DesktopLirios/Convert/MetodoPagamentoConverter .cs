using System;
using System.Globalization;
using System.Windows.Data;

public class MetodoPagamentoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int metodoPagamento)
        {
            switch (metodoPagamento)
            {
                case 0:
                    return "A Pagar";
                case 1:
                    return "Dinheiro";
                case 2:
                    return "Pix";
                case 3:
                    return "Debito";
                case 4:
                    return "Credito a Vista";
                case 5:
                    return "Parcelado";
                case 6:
                    return "Credito Parcelado";
                default:
                    return "Desconhecido";
            }
        }

        return "Desconhecido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}