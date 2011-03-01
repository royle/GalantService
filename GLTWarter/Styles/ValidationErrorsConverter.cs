using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace GLTWarter.Styles
{
    [ValueConversion(typeof(ReadOnlyObservableCollection<ValidationError>), typeof(String))]
    public class ValidationErrorsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<ValidationError>)
            {
                ReadOnlyObservableCollection<ValidationError> ves = (ReadOnlyObservableCollection<ValidationError>)value;
                string s = string.Empty;
                foreach (ValidationError ve in ves) {
                    if (ve.Exception != null && ve.Exception.InnerException != null)
                    {
                        s = ve.Exception.InnerException.Message;
                    }
                    else
                    {
                        s = ve.ErrorContent.ToString();
                    }
                    return s;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
