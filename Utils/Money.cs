using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Utils
{
    public class Money : PropertyChangedHelper, IComparable
    {
        public Money(double amount, Currency currency = Currency.EUR)
        {
            Amount = amount;
            Currency = currency;
        }

        public double Amount
        {
            get { return GetValue(() => Amount); }
            set { SetValue(value); }
        }

        public string AmountString
        {
            get { return ToString(); }
            set
            {
                string t = value;
                if (t.Contains(GetCurrencySymbol()))
                    t = t.Replace(GetCurrencySymbol(), "");
                t = t.Trim();
                try
                {
                    Amount = Convert.ToDouble(t);
                    SetValue(value);
                }
                catch
                {
                    Amount = 0;
                    SetValue(0);
                }
               
            }
        }

        public Currency Currency
        {
            get { return GetValue(() => Currency); }
            set { SetValue(value); }
        }

        public string GetCurrencySymbol()
        {
            string cur = "";
            switch (Currency)
            {
                case Currency.EUR:
                    cur = "€";
                    break;
                case Currency.CHF:
                    cur = "CHF";
                    break;
                case Currency.USD:
                    cur = "$";
                    break;
            }
            return cur;
        }

        public double GetRoundedValue()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 2;
            return Double.Parse(Amount.ToString("n", nfi), nfi);
        }

        public override string ToString()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 2;
            return Amount.ToString("n", nfi) + " " + GetCurrencySymbol();
        }

        public override bool Equals(object obj)
        {
            if (obj is Money)
                return (obj as Money) == this;
            return false;
        }

        #region "Operators"
        public static Money operator +(Money money1, Money money2)
        {
            if (money1 != null && money2 != null)
            {
                if (money1.Currency != money2.Currency)
                {
                    throw new Exception("Unterschiedliche Währungen");
                }
                return new Money(money1.Amount + money2.Amount, money1.Currency);
            }
            return new Money(0);
        }
        public static Money operator +(Money money1, double amount2)
        {
            return new Money(money1.Amount + amount2, money1.Currency);
        }

        public static Money operator -(Money money1, Money money2)
        {
            if (money1.Currency != money2.Currency)
            {
                throw new Exception("Unterschiedliche Währungen");
            }
            return new Money(money1.Amount - money2.Amount, money1.Currency);
        }
        public static Money operator -(Money money1, double amount2)
        {
            return new Money(money1.Amount - amount2, money1.Currency);
        }

        public static Money operator *(Money money1, Money money2)
        {
            if (money1.Currency != money2.Currency)
            {
                throw new Exception("Unterschiedliche Währungen");
            }
            return new Money(money1.Amount * money2.Amount, money1.Currency);
        }

        public static Money operator *(Money money1, double amount2)
        {
            return new Money(money1.Amount * amount2, money1.Currency);
        }

        public static Money operator /(Money money1, Money money2)
        {
            if (money1.Currency != money2.Currency)
            {
                throw new Exception("Unterschiedliche Währungen");
            }
            return new Money(money1.Amount / money2.Amount, money1.Currency);
        }

        public static Money operator /(Money money1, double amount2)
        {
            return new Money(money1.Amount / amount2, money1.Currency);
        }

        public static bool operator <(Money money1, Money money2)
        {
            return money1.Amount < money2.Amount;
        }

        public static bool operator <(Money money1, double money2)
        {
            return money1.Amount < money2;
        }

        public static bool operator >(Money money1, Money money2)
        {
            return money1.Amount > money2.Amount;
        }

        public static bool operator >(Money money1, double money2)
        {
            return money1.Amount > money2;
        }

        #endregion

        public int CompareTo(object obj)
        {
            if (obj is Money)
                return Convert.ToInt32(Amount - (obj as Money).Amount);
            return 0;
        }

        //public override int GetHashCode()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + Amount.GetHashCode();
        //        hash = hash * 23 + GetCurrencySymbol().GetHashCode();
        //        return hash;
        //    }
        //}

        public int GetStorageHash()
        {
            return GetHashCode();
        }
    }

    public enum Currency
    {
        EUR = 0,
        USD = 1,
        CHF = 2
    }
}
