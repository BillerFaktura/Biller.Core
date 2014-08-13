using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Core.Utils
{
    public class EAddress : Address, Interfaces.IXMLStorageable
    {
        public EAddress() : base()
        {
            // insert empty values to avoid null exceptions
            AddressDescription = "";
        }

        public string AddressDescription
        {
            get { return GetValue(() => AddressDescription); }
            set { SetValue(value); }
        }

        public override System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = base.GetXElement();
            objectnode.Name = XElementName;
            objectnode.Add(new XElement("AddressDescription", AddressDescription));
            return objectnode;
        }

        public override void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Name of XElement was " + source.Name + " but expected " + XElementName);

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
            AddressDescription = source.Element("AddressDescription").Value;
        }

        public override string XElementName
        {
            get { return "EAddress"; }
        }

        public override bool Equals(object obj)
        {
            if (obj is EAddress)
                if ((obj as EAddress).AddressDescription==AddressDescription)
                    return true;
            return false;
        }

        public override string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public override string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public override Interfaces.IXMLStorageable GetNewInstance()
        {
            return new EAddress();
        }

        //public override int GetStorageHash()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + AddressDescription.GetHashCode();
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
