using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ATM_TestTask
{
    public class MultiCommandConverter : IMultiValueConverter
    {
        private List<object> _value = new List<object>();

        public object Convert(object[] value, Type targetType,
            object parameter, CultureInfo culture)
        {
            _value.AddRange(value);
            return new CommonCommand<object>(GetCompoundExecute(), GetCompoundCanExecute());
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, CultureInfo culture)
        {
            return null;
        }
        private Action<object> GetCompoundExecute()
        {
            return (parameter) =>
            {
                foreach (CommonCommand<object> command in _value)
                {
                    if (command != default(CommonCommand<object>))
                        command.Execute(parameter);
                }
            };
        }
        private Predicate<object> GetCompoundCanExecute()
        {
            return (parameter) =>
            {
                bool res = true;
                foreach (var command in _value)
                    if (command != default(CommonCommand<object>) && command is CommonCommand<object>)
                        res &= ((CommonCommand<object>)command).CanExecute(parameter);
                return res;
            };
        }
    }
}
