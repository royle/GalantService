using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using Galant.DataEntity;

namespace GLTWarter.Data
{
    class DummyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(object), typeof(String))]
    class ValueTypeBindingConverter : IValueConverter
    {
        static Dictionary<XmlDataProvider, Dictionary<string, string>> cache = new Dictionary<XmlDataProvider, Dictionary<string, string>>();
        public static string Convert(object value, XmlDataProvider parameter)
        {
            string searchValue = null;
            if (value == null) return null;
            if (value is int)
            {
                searchValue = ((int)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (value is string)
            {
                searchValue = value.ToString();
            }
            else
            {
                searchValue = value.ToString();
            }
            if (searchValue == null) return null;

            XmlDataProvider xdp = parameter as XmlDataProvider;
            if (xdp != null && xdp.Document != null)
            {
                if (!cache.ContainsKey(xdp))
                {
                    cache.Add(xdp, new Dictionary<string, string>());
                }
                if (cache[xdp].ContainsKey(searchValue))
                {
                    return cache[xdp][searchValue];
                }
                else
                {
                    XmlNode n = xdp.Document.SelectSingleNode("//*[Value='" + searchValue + "']");
                    if (n != null)
                    {
                        n = n.SelectSingleNode("Text");
                        if (n != null)
                        {
                            cache[xdp].Add(searchValue, n.InnerText);
                            return n.InnerText;
                        }
                    }
                    else
                    {
                        cache[xdp].Add(searchValue, searchValue);
                        return searchValue;
                    }
                }
            }
            return searchValue;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is XmlDataProvider && value != null)
            {
                return Convert(value, parameter as XmlDataProvider);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    [ValueConversion(typeof(Array), typeof(Visibility))]
    class ArrayCountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Array == null) return Visibility.Collapsed;
            if (((Array)value).Length == 0) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(Array), typeof(Visibility))]
    class ArrayNotCountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Array == null) return Visibility.Visible;
            if (((Array)value).Length == 0) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    class IsNullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    class IsNotNullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    [ValueConversion(typeof(string), typeof(Visibility))]
    class IsStringEmptyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string.IsNullOrEmpty(value as string)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    class IsNotStringEmptyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!string.IsNullOrEmpty(value as string)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    class StringBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as string))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return parameter != null ? parameter as string : "Set";
                }
                else
                {
                    return string.Empty;
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    class StringRadioBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || value == null) return false;
            return (value.ToString() == parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    class BooleanNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            else if (value is string)
            {
                return !(string.IsNullOrEmpty((string)value) || (string)value == "0") ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanNotVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (!(bool)value) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            else if (value is string)
            {
                return (string.IsNullOrEmpty((string)value) || (string)value == "0") ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanNotAllVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object value in values)
            {
                if (value is bool && (bool)value)
                {
                    return (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
                }
                else if (value is string && (!string.IsNullOrEmpty((string)value) && (string)value != "0"))
                {
                    return (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
                }
            }
            return Visibility.Visible;        
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(Visibility))]
    class IsDateTimeMinVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                return (DateTime)value == DateTime.MinValue ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(Visibility))]
    class IsDateTimeNotMinVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                return (DateTime)value != DateTime.MinValue ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    class IsZeroVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value == 0) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            else if (value is Int64)
            {
                return ((Int64)value == 0) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    class IsZeroNotVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value != 0) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            else if (value is Int64)
            {
                return ((Int64)value != 0) ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }    
  
    [ValueConversion(typeof(int), typeof(bool))]
    class IsNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value != 0);
            } else if (value is Int64)
            {
                return ((Int64)value != 0);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    
    [ValueConversion(typeof(object), typeof(bool))]
    class IsNullBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(object), typeof(bool))]
    class IsNotNullBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class SelectFirstConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return DependencyProperty.UnsetValue;
            foreach (object o in values) { if (o != null) return o; }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class SelectFirstAsArrayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return DependencyProperty.UnsetValue;
            foreach (object o in values) { if (o != null) return new object[] { o }; }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string[]), typeof(string))]
    class ArrayStringConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values as string[] == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(parameter as string ?? Environment.NewLine, values as string[]);
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string[]), typeof(string))]
    class ArrayFirstStringConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values as string[] == null || (values as string[]).Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return (values as string[])[0] ?? string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(string))]
    class ExampleConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", values);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(string))]
    class NullStringConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            return values as string ?? "[[NullString]]";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string == "[[NullString]]") ? null : value;
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class IntDisplayConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values as int?).HasValue)
            {
                return (values as int?).Value.ToString(CultureInfo.CurrentCulture);
            }
            else if ((values as Int64?).HasValue)
            {
                return (values as Int64?).Value.ToString(CultureInfo.CurrentCulture);
            }
            return string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    class IntGreaterVisibilityConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values as int?).HasValue && (parameter as int?).HasValue)
            {
                return (values as int?).Value > (parameter as int?).Value ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    class IntLesserVisibilityConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values as int?).HasValue && (parameter as int?).HasValue)
            {
                return (values as int?).Value < (parameter as int?).Value ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class IsSameBaseDataBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return DependencyProperty.UnsetValue;
            return IsSame(values[0] as BaseData, values[1] as BaseData);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        internal static bool IsSame(BaseData a, BaseData b)
        {
            if (a == null || b == null) return false;
            if (a == b) return true;
            if (a.GetType().Name != b.GetType().Name) return false;
            if (a.QueryId == null || b.QueryId == null) return false;
            if (a.QueryId == b.QueryId) return true;
            return false;
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class IsSameBaseDataNotBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return DependencyProperty.UnsetValue;
            return !IsSameBaseDataBooleanConverter.IsSame(values[0] as BaseData, values[1] as BaseData);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class IsSameBaseDataVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return DependencyProperty.UnsetValue;
            return IsSameBaseDataBooleanConverter.IsSame(values[0] as BaseData, values[1] as BaseData) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    class IsSameBaseDataNotVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return DependencyProperty.UnsetValue;
            return IsSameBaseDataBooleanConverter.IsSame(values[0] as BaseData, values[1] as BaseData) ?
                Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }





    [ValueConversion(typeof(Decimal), typeof(string))]
    class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as decimal? != null)
            {
                decimal? amt = value as decimal?;
                if (amt.HasValue)
                {
                    return amt.Value.ToString("F2", CultureInfo.InvariantCulture);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal ret;
            return string.IsNullOrEmpty(value as string) ? null :
                (decimal.TryParse(value as string, out ret) ? (decimal?)ret : null);
        }
    }


    [ValueConversion(typeof(DateTime), typeof(string))]
    class EventDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string != null)
            {
                value = PgsqlToDate(value as string);
            }
            if (value as DateTime? != null)
            {
                if ((DateTime)value == DateTime.MinValue) return string.Empty;
                return ((DateTime)value).ToLocalTime().ToString(Resource.dateEventConvention);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static DateTime? PgsqlToDate(string input)
        {
            DateTime? ret = null;
            const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFFFFFF";
            try
            {
                ret = DateTime.ParseExact(input, dateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
            }
            catch (FormatException)
            {
            }
            return ret;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    class EventDateLongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string != null)
            {
                value = EventDateConverter.PgsqlToDate(value as string);
            }
            if (value as DateTime? != null)
            {
                if ((DateTime)value == DateTime.MinValue) return string.Empty;
                return ((DateTime)value).ToLocalTime().ToString(Resource.dateEventLongConvention);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(Entity), typeof(string))]
    class EntityNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Entity != null)
            {
                return Convert((Entity)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static string Convert(Entity e)
        {
            return string.IsNullOrEmpty(e.Alias) ?
                string.Format(CultureInfo.CurrentCulture, "{0}", e.FullName ?? string.Empty) :
                string.Format(CultureInfo.CurrentCulture, "{0}", e.FullName ?? string.Empty, e.Alias);
        }
    }

    [ValueConversion(typeof(Entity), typeof(string))]
    class EntityPhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Entity != null)
            {
                if (!string.IsNullOrEmpty(((Entity)value).CellPhoneOne))
                {
                    return ((Entity)value).CellPhoneOne;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(Entity), typeof(string))]
    class EntityNameForRouteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value as Entity);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static string Convert(Entity e)
        {
            return e == null ? "转换错误" : EntityNameConverter.Convert(e);
        }
    }

    [ValueConversion(typeof(Entity), typeof(string))]
    class EntityNameForIndividualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value as Entity);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static string Convert(Entity e)
        {
            return e == null ? "转换错误" : EntityNameConverter.Convert(e);
        }
    }



    [ValueConversion(typeof(string), typeof(Entity))]
    class EntityAliasInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Entity != null)
            {
                return ((Entity)value).Alias;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as String))
            {
                return null;
            }
            else
            {
                Entity data = new Entity();
                data.Alias = value as String;
                data.EntityId = null;
                return data;
            }
        }
    }



    [ValueConversion(typeof(bool), typeof(Entity))]
    class EntityLastMileInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? null : new Entity();
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    class StringUniverseDateTimeInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringDateTimeInputConverter.Convert(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as DateTime?).HasValue)
            {
                return StringUniverseDateTimeInputConverter.ConvertBack((value as DateTime?).Value);
            }
            return DependencyProperty.UnsetValue;
        }

        public static DateTime? Convert(string value)
        {
            DateTime date;
            if (string.IsNullOrEmpty(value)) return null;
            if (DateTime.TryParse(value as string, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertBack(DateTime value)
        {
            return value.ToUniversalTime().ToString("u", CultureInfo.InvariantCulture);
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    class StringDateTimeInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringDateTimeInputConverter.Convert(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as DateTime?).HasValue)
            {
                return StringDateTimeInputConverter.ConvertBack((value as DateTime?).Value);
            }
            return DependencyProperty.UnsetValue;
        }

        public static DateTime? Convert(string value)
        {
            DateTime date;
            if (string.IsNullOrEmpty(value)) return null;
            if (DateTime.TryParse(value as string, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertBack(DateTime value)
        {
            return value.ToString("s", CultureInfo.InvariantCulture);
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    class DateTimeInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as DateTime?).HasValue)
            {
                return (value as DateTime?).Value.ToString("s", CultureInfo.InvariantCulture);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date;
            if (string.IsNullOrEmpty(value as string)) return null;
            if (DateTime.TryParse(value as string, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out date))
            {
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }


    [ValueConversion(typeof(int?), typeof(string))]
    class IntInputConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values as int?).HasValue)
            {
                return (values as int?).Value.ToString(CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret;
            return string.IsNullOrEmpty(value as string) ? null :
                (int.TryParse(value as string, out ret) ? (int?)ret : null);
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class DoubleDigitsIntInputConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values as int?).HasValue)
            {
                return (values as int?).Value.ToString("D2", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret;
            return string.IsNullOrEmpty(value as string) ? null :
                (int.TryParse(value as string, out ret) ? (int?)ret : null);
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class SubstringStartConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            int length;
            if (values != null && int.TryParse(parameter as string, out length))
            {
                return values.ToString().Substring(0, length);
            }
            return string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Route), typeof(string))]
    class RouteNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Route != null)
            {
                return Convert((Route)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static string Convert(Route e)
        {
            return
                string.Format(
                CultureInfo.CurrentCulture, Resource.converterRouteName,
                e.RountName ?? string.Empty,
                e.RouteId);
        }
    }
  
 }
