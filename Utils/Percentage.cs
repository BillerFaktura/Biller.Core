using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Utils
{
    public class Percentage : PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Percentage()
        {
            Amount = 0;
        }

        /// <summary>
        /// Value between 0 and 1
        /// </summary>
        public double Amount
        {
            get { return GetValue(() => Amount); }
            set { SetValue(value); RaiseUpdateManually("PercentageString"); }
        }

        public string PercentageString
        {
            get 
            {
                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = "";
                nfi.NumberDecimalSeparator = ",";
                nfi.NumberDecimalDigits = 2;
                return (Amount * 100).ToString("n", nfi) + " %";
            }
            set
            {
                string t = value;
                if (t.Contains("%"))
                    t = t.Replace("%", "");
                t = t.Trim();
                Amount = Convert.ToDouble(t) / 100;
            }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new System.Xml.Linq.XElement(XElementName, Amount);
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            string t = source.Value;
            if (t.Contains("%"))
                t = t.Replace("%", "");
            t = t.Trim();
            Amount = Convert.ToDouble(t) / 100;
        }

        public string XElementName
        {
            get { return "Percentage"; }
        }

        public string ID
        {
            get { return ""; }
        }

        public string IDFieldName
        {
            get { return ""; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Percentage();
        }

        //public int GetStorageHash()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + PercentageString.GetHashCode();
        //        return hash;
        //    }
        //}
    }
}
