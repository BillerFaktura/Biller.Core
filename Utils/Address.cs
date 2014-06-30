using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Core.Utils
{
    public class Address : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Address()
        {
            //Insert empty values to avoid null exceptions
            Salutation = ""; Title = ""; Forename = ""; Surname = "";
            CompanyName = ""; Street = ""; HouseNumber = ""; Zip = "";
            City = ""; Country = ""; Addition = "";
        }

        public string Salutation
        {
            get { return GetValue(() => Salutation); }
            set { SetValue(value); }
        }

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(value); }
        }

        public string Forename
        {
            get { return GetValue(() => Forename); }
            set { SetValue(value); }
        }

        public string Surname
        {
            get { return GetValue(() => Surname); }
            set { SetValue(value); }
        }

        public string CompanyName
        {
            get { return GetValue(() => CompanyName); }
            set { SetValue(value); }
        }

        public string Street
        {
            get { return GetValue(() => Street); }
            set { SetValue(value); }
        }

        public string HouseNumber
        {
            get { return GetValue(() => HouseNumber); }
            set { SetValue(value); }
        }

        public string Zip
        {
            get { return GetValue(() => Zip); }
            set { SetValue(value); }
        }

        public string City
        {
            get { return GetValue(() => City); }
            set { SetValue(value); }
        }

        public string Country
        {
            get { return GetValue(()=> Country); }
            set { SetValue(value); }
        }

        public string Addition
        {
            get { return GetValue(() => Addition); }
            set { SetValue(value); }
        }

        public virtual System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = new XElement(XElementName);
            objectnode.Add(new XElement("Salutation", Salutation), new XElement("Title", Title), new XElement("Forename", Forename),
                new XElement("Surname", Surname), new XElement("CompanyName", CompanyName), new XElement("Street", Street),
                new XElement("HouseNumber", HouseNumber), new XElement("Zip", Zip), new XElement("City", City),
                new XElement("Country", Country), new XElement("Addition", Addition));
            return objectnode;
        }

        public virtual void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("The given element is not named Address and will not be parsed.");

            Salutation = source.Element("Salutation").Value;
            Title = source.Element("Title").Value;
            Forename = source.Element("Forename").Value;
            Surname = source.Element("Surname").Value;
            CompanyName = source.Element("CompanyName").Value;
            Street = source.Element("Street").Value;
            HouseNumber = source.Element("HouseNumber").Value;
            Zip = source.Element("Zip").Value;
            City = source.Element("City").Value;
            Country = source.Element("Country").Value;
            Addition = source.Element("Addition").Value;
        }

        public virtual string XElementName
        {
            get { return "Address"; }
        }

        public string OneLineString
        {
            get
            {
                var names = (((Salutation + " " + Title + " ").Trim() + " " + Forename).Trim() + " " + Surname).Trim();
                if (!String.IsNullOrEmpty(names))
                    names += ", ";
                
                var street = (Street + " " + HouseNumber).Trim();
                if (!String.IsNullOrEmpty(street))
                    street += ", ";

                var city = (Zip + " " + City).Trim();
                if (!String.IsNullOrEmpty(city))
                    city += ", ";

                var output = "";
                if (!String.IsNullOrEmpty(CompanyName))
                    output = CompanyName + ", ";
                output += names + street;
                if (!String.IsNullOrEmpty(Addition))
                    output += Addition + ", ";
                output += city;
                if (!String.IsNullOrEmpty(Country))
                    output += Country;

                if (output.EndsWith(", "))
                    output=output.Remove(output.LastIndexOf(", "));
                return output;
            }
        }

        public List<string> AddressStrings
        {
            get
            {
                var output = new List<string>();
                if (!String.IsNullOrEmpty(CompanyName))
                {
                    output.Add(CompanyName);

                    var names = (((Salutation + " " + Title).Trim() + " " + Forename).Trim() + " " + Surname).Trim();
                    if (!String.IsNullOrEmpty(names))
                        output.Add(names);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Salutation))
                        output.Add(Salutation);

                    var names = ((Title + " " + Forename.Trim()).Trim() + " " + Surname).Trim();
                    if (!String.IsNullOrEmpty(names))
                        output.Add(names);
                }


                if (!String.IsNullOrEmpty(Addition))
                    output.Add(Addition);

                var street = (Street + " " + HouseNumber).Trim();
                if (!String.IsNullOrEmpty(street))
                    output.Add(street);

                var city = (Zip + " " + City).Trim();
                if (!String.IsNullOrEmpty(city))
                    output.Add(city);

                if (!String.IsNullOrEmpty(Country))
                    output.Add(Country);

                return output;
            }
        }

        public virtual string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public virtual string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Address();
        }

        //public virtual int GetStorageHash()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + Title.GetHashCode();
        //        hash = hash * 23 + Forname.GetHashCode();
        //        hash = hash * 23 + Surname.GetHashCode();
        //        hash = hash * 23 + Street.GetHashCode();
        //        hash = hash * 23 + HouseNumber.GetHashCode();
        //        hash = hash * 23 + Zip.GetHashCode();
        //        hash = hash * 23 + City.GetHashCode();
        //        hash = hash * 23 + Country.GetHashCode();
        //        hash = hash * 23 + Addition.GetHashCode();
        //        return hash;
        //    }
        //}
    }
}
