using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Core.Utils
{
    public class EMoney : Money, Interfaces.IXMLStorageable
    {
        public EMoney(double amount, bool Gross = true, Currency currency = Currency.EUR)
            : base(amount, currency)
        {
            IsGross = Gross;
        }

        public bool IsGross { get { return GetValue(() => IsGross); } set { SetValue(value); } }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new XElement(XElementName, new XAttribute("Amount", Amount), new XAttribute("IsGross", IsGross), new XAttribute("Currency", Currency));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            double temp;
            if (double.TryParse(source.Attribute("Amount").Value, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                Amount = temp;
            IsGross = bool.Parse(source.Attribute("IsGross").Value);
            Currency = (Currency)Enum.Parse(typeof(Currency),source.Attribute("Currency").Value);
        }

        public string XElementName
        {
            get { return "Money"; }
        }

        public string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new EMoney(0);
        }

        #region "Operators"
        public static EMoney operator +(EMoney money1, EMoney money2)
        {
            if (money1 != null && money2 != null)
            {
                if (money1.Currency != money2.Currency)
                    throw new Exception("Unterschiedliche Währungen");
                return new EMoney(money1.Amount + money2.Amount, money1.IsGross, money1.Currency);
            }
            return new EMoney(0);
        }
        public static EMoney operator +(EMoney money1, double amount2)
        {
            return new EMoney(money1.Amount + amount2, money1.IsGross, money1.Currency);
        }

        public static EMoney operator -(EMoney money1, EMoney money2)
        {
            if (money1.Currency != money2.Currency)
                throw new Exception("Unterschiedliche Währungen");
            return new EMoney(money1.Amount - money2.Amount, money1.IsGross, money1.Currency);
        }
        public static EMoney operator -(EMoney money1, double amount2)
        {
            return new EMoney(money1.Amount - amount2, money1.IsGross, money1.Currency);
        }

        public static EMoney operator *(EMoney money1, EMoney money2)
        {
            if (money1.Currency != money2.Currency)
                throw new Exception("Unterschiedliche Währungen");
            return new EMoney(money1.Amount * money2.Amount, money1.IsGross, money1.Currency);
        }

        public static EMoney operator *(EMoney money1, double amount2)
        {
            return new EMoney(money1.Amount * amount2, money1.IsGross, money1.Currency);
        }

        public static EMoney operator /(EMoney money1, EMoney money2)
        {
            if (money1.Currency != money2.Currency)
                throw new Exception("Unterschiedliche Währungen");
            return new EMoney(money1.Amount / money2.Amount, money1.IsGross, money1.Currency);
        }

        public static EMoney operator /(EMoney money1, double amount2)
        {
            return new EMoney(money1.Amount / amount2, money1.IsGross, money1.Currency);
        }

        public static bool operator <(EMoney money1, EMoney money2)
        {
            return money1.Amount < money2.Amount;
        }

        public static bool operator <(EMoney money1, double money2)
        {
            return money1.Amount < money2;
        }

        public static bool operator >(EMoney money1, EMoney money2)
        {
            return money1.Amount > money2.Amount;
        }

        public static bool operator >(EMoney money1, double money2)
        {
            return money1.Amount > money2;
        }
        #endregion
    }
}
