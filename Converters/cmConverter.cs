using Biller.Core.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Biller.Core.Converters
{
    /// <summary>
    /// Converts a double to specific decimal format with a unit
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class cmConverter : IValueConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Unit cmUnit { get; private set; }

        public cmConverter()
        {
            if (cmConverter.cmUnit == null)
                cmConverter.cmUnit = new Unit() { DecimalDigits = 2, DecimalSeperator = ",", Name = "Centimeter", ShortName = "cm", ThousandSeperator = "" };
        }

        #region "IValueConverter Members"
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return cmConverter.cmUnit.ValueToString((double)value);
            }
            catch (Exception e)
            {
                logger.ErrorException("Error converting " + value.ToString(), e);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return cmConverter.cmUnit.StringToValue(value.ToString());
            }
            catch (Exception e)
            {
                logger.ErrorException("Error converting back from " + value.ToString(), e);
            }
            return value;
        }
        #endregion
    }
}
