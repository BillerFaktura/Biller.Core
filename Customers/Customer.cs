using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Core.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public class Customer : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Customer()
        {
            //insert empty values to avoid null exceptions.
            CustomerID = ""; Contact = new Utils.Contact(); MainAddress = new Utils.Address(); Pricegroup = 0; IsCompany = false;
            ExtraAddresses = new ObservableCollection<Utils.EAddress>(); DefaultPaymentMethode = new Utils.PaymentMethode();
        }

        /// <summary>
        /// The unique identifier of the customer.
        /// </summary>
        public string CustomerID
        {
            get { return GetValue(() => CustomerID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Determines wheter the customer is a company (for b2b) or a private person.
        /// </summary>
        public bool IsCompany
        {
            get { return GetValue(() => IsCompany); }
            set { SetValue(value); }
        }

        public Utils.Contact Contact
        {
            get { return GetValue(() => Contact); }
            set { SetValue(value); }
        }

        public Utils.Address MainAddress
        {
            get { return GetValue(() => MainAddress); }
            set { SetValue(value); }
        }

        public ObservableCollection<Utils.EAddress> ExtraAddresses
        {
            get { return GetValue(() => ExtraAddresses); }
            set { SetValue(value); }
        }

        public Utils.PaymentMethode DefaultPaymentMethode
        {
            get { return GetValue(() => DefaultPaymentMethode); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Shows a combination of properties to display a short and meaningfull description of the customername.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(MainAddress.CompanyName) && !String.IsNullOrEmpty(MainAddress.Surname))
                    return MainAddress.Surname;
                if (!String.IsNullOrEmpty(MainAddress.CompanyName) && !String.IsNullOrEmpty(MainAddress.Surname))
                    return MainAddress.CompanyName + ", " + MainAddress.Surname;
                if (!String.IsNullOrEmpty(MainAddress.CompanyName) && String.IsNullOrEmpty(MainAddress.Surname))
                    return MainAddress.CompanyName;

                if (!String.IsNullOrEmpty(MainAddress.Forename))
                    return MainAddress.Forename;
                if (!String.IsNullOrEmpty(MainAddress.Addition))
                    return MainAddress.Addition;

                return default(string);
            }
        }

        /// <summary>
        /// <list type="bullet">
        /// <item>0 for pricegroup 1</item>
        /// <item>1 for pricegroup 2</item>
        /// <item>2 for pricegroup 3</item>
        /// </list>
        /// </summary>
        public int Pricegroup
        {
            get { return GetValue(() => Pricegroup); }
            set { SetValue(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement GetXElement()
        {
            var element = new XElement(XElementName);
            element.Add(new XElement("CustomerID", CustomerID), new XElement("IsCompany", IsCompany), Contact.GetXElement(), new XElement("PriceGroup", Pricegroup),
                new XElement("MainAddress", MainAddress.GetXElement()), new XElement("DefaultPaymentMethode", DefaultPaymentMethode.Name),
                new XElement("DisplayName", DisplayName), new XElement("ExtraAddresses", from addresses in ExtraAddresses select addresses.GetXElement()));
            return element;
        }

        /// <summary>
        /// Reads an XElement from an <see cref="IDatabase"/> to override values of the current object.\n
        /// <b>ATTENTION: Does not parse <see cref="DefaultPaymentMethode"/> of type <see cref="PaymentMethode"/>.</b>
        /// </summary>
        /// <param name="source"></param>
        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Name of XElement was " + source.Name + " but expected " + XElementName);

            CustomerID = source.Element("CustomerID").Value;
            IsCompany = bool.Parse(source.Element("IsCompany").Value);
            Pricegroup = int.Parse(source.Element("PriceGroup").Value);

            var contacttemp = new Biller.Core.Utils.Contact();
            contacttemp.ParseFromXElement(source.Element("Contact"));
            Contact = contacttemp;

            var mainaddresstemp = new Core.Utils.Address();
            mainaddresstemp.ParseFromXElement(source.Element("MainAddress").Element(mainaddresstemp.XElementName));
            MainAddress = mainaddresstemp;

            ExtraAddresses = new ObservableCollection<Utils.EAddress>();
            var addresslist = source.Element("ExtraAddresses").Elements();
            foreach (XElement item in addresslist)
            {
                var tempaddress = new Core.Utils.EAddress();
                tempaddress.ParseFromXElement(item);
                ExtraAddresses.Add(tempaddress);
            }
                
        }

        /// <summary>
        /// Identifier string that is used by <see cref="ParseFromXElement"/> and <see cref="GetXElement"/> to define the XElement's name.
        /// </summary>
        public string XElementName
        {
            get { return "Customer"; }
        }

        /// <summary>
        /// Returns <see cref="CustomerID"/>.
        /// </summary>
        public string ID
        {
            get { return CustomerID; }
        }

        /// <summary>
        /// Returns a new instance of <see cref="Customer"/>.
        /// </summary>
        /// <returns></returns>
        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Customer();
        }

        /// <summary>
        /// Returns "CustomerID".
        /// </summary>
        public string IDFieldName
        {
            get { return "CustomerID"; }
        }
    }
}
