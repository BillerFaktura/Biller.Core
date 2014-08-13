using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Linq;


namespace Biller.Core.Utils
{
    /// <summary>
    /// Represents a sales unit. The identifier of an object is <see cref="Name"/>.
    /// </summary>
    public class Unit : PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        private NumberFormatInfo nfi;

        public Unit()
        {
            //Specify empty and default values to avoid null Exceptions
            Name = ""; ShortName = "";
            DecimalDigits = 3; ThousandSeperator = "";
            DecimalSeperator = ",";
        }

        /// <summary>
        /// The name of the unit.
        /// </summary>
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(value); }
        }

        /// <summary>
        /// The abbreviation of <see cref="Name"/>.
        /// </summary>
        public string ShortName
        {
            get { return GetValue(() => ShortName); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Determines how many digits are shown behind the <see cref="DecimalSeperator"/>.
        /// </summary>
        public int DecimalDigits
        {
            get { return GetValue(() => DecimalDigits); }
            set { SetValue(value); }
        }

        /// <summary>
        /// The symbol that should be displayed as thousand seperator. In Europe it's '.' or ' ' (whitespace), in the USA and UK ','.
        /// </summary>
        public string ThousandSeperator
        {
            get { return GetValue(() => ThousandSeperator); }
            set { SetValue(value); }
        }

        /// <summary>
        /// The symbol that shall be displayed as seperator. In Europe the common symbol is ','. In the USA and UK it's '.'.
        /// </summary>
        public string DecimalSeperator
        {
            get { return GetValue(() => DecimalSeperator); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Shows a number formatted with the values from this classes fields. 
        /// </summary>
        /// <param name="value">The value you want to format.</param>
        /// <returns></returns>
        public string ValueToString(double value)
        {
            if (nfi == null)
                nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

            nfi.NumberGroupSeparator = ThousandSeperator;
            nfi.NumberDecimalSeparator = DecimalSeperator;
            nfi.NumberDecimalDigits = DecimalDigits;
            return value.ToString("n", nfi) + " " + ShortName;
        }

        public double StringToValue(string value)
        {
            if (nfi == null)
                nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

            nfi.NumberGroupSeparator = ThousandSeperator;
            nfi.NumberDecimalSeparator = DecimalSeperator;
            nfi.NumberDecimalDigits = DecimalDigits;
            value = value.Replace(ShortName, "");
            double parsedValue = 0;
            double.TryParse(value, NumberStyles.Number, nfi, out parsedValue);
                return parsedValue;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string ToString()
        {
            return "{Units: Name=" + Name + ", ShortName=" + ShortName + "}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + ShortName.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Unit)
            {
                if ((obj as Unit).Name == Name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = new XElement(XElementName);
            objectnode.Add(new XElement("Name", Name));
            objectnode.Add(new XElement("ShortName", ShortName), new XElement("DecimalDigits", DecimalDigits),
                new XElement("ThousandSeperator", ThousandSeperator), new XElement("DecimalSeperator", DecimalSeperator));
            return objectnode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != "Unit")
                throw new Exception("The given source element is not named Unit (but " + source.Name + ") and will not be parsed!");

            if (source.Attribute("Name") != null)
            {
                Name = source.Attribute("Name").Value;
            }
            else
            {
                Name = source.Element("Name").Value;
            }
            
            ShortName = source.Element("ShortName").Value;
            int temp;
            var parse = (int.TryParse(source.Element("DecimalDigits").Value, out temp)) ? DecimalDigits = temp : DecimalDigits = 3;
            ThousandSeperator = source.Element("ThousandSeperator").Value;
            DecimalSeperator = source.Element("DecimalSeperator").Value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string XElementName
        {
            get { return "Unit"; }
        }

        public string ID
        {
            get { return Name; }
        }

        public string IDFieldName
        {
            get { return "Name"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Unit();
        }
    }
}
