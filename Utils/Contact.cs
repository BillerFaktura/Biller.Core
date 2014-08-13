using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Core.Utils
{
    /// <summary>
    /// The Contact class saves the most important information to communicate with the <see cref="Customer"/>.
    /// </summary>
    public class Contact : PropertyChangedHelper, Interfaces.IXMLStorageable
    {

        /// <summary>
        /// Initializes a new instance of <see cref="Contact"/>.
        /// </summary>
        public Contact()
        {
            // insert empty values to avoid null exceptions.
            Phone1 = ""; Phone2 = ""; Fax1 = ""; Fax2 = ""; Mobile1 = ""; Mobile2 = "";
            Facebook = ""; Twitter = ""; Mail1 = ""; Mail2 = "";
        }

        /// <summary>
        /// Represents a phone number. No format restriction. PlugIns can use this information to start a call via Skype, Lync...
        /// </summary>
        public string Phone1
        {
            get { return GetValue(() => Phone1); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a phone number. No format restriction. PlugIns can use this information to start a call via Skype, Lync...
        /// </summary>
        public string Phone2
        {
            get { return GetValue(() => Phone2); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a phone number where you can send faxes to. No format restriction.
        /// </summary>
        public string Fax1
        {
            get { return GetValue(() => Fax1); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a phone number where you can send faxes to. No format restriction.
        /// </summary>
        public string Fax2
        {
            get { return GetValue(() => Fax2); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a mobile phone number. No format restriction. PlugIns can use this information to start a call via Skype, Lync...
        /// </summary>
        public string Mobile1
        {
            get { return GetValue(() => Mobile1); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a mobile phone number. No format restriction. PlugIns can use this information to start a call via Skype, Lync...
        /// </summary>
        public string Mobile2
        {
            get { return GetValue(() => Mobile2); }
            set { SetValue(value); }
        }

        public string Mail1
        {
            get { return GetValue(() => Mail1); }
            set { SetValue(value); }
        }

        public string Mail2
        {
            get { return GetValue(() => Mail2); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Field to store the username of the facebook-user\n
        /// Normally the url is like: https://www.facebook.com/username \n
        /// The field's value should be "username" in this case
        /// </summary>
        public string Facebook
        {
            get { return GetValue(() => Facebook); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Field to store the username of the twitter-user\n
        /// Normally the url is like: https://twitter.com/username \n
        /// The field's value should be "username" in this case
        /// </summary>
        public string Twitter
        {
            get { return GetValue(() => Twitter); }
            set { SetValue(value); }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = new XElement(XElementName);
            objectnode.Add(new XElement("Phone1", Phone1), new XElement("Phone2", Phone2), new XElement("Fax1", Fax1),
                new XElement("Fax2", Fax2), new XElement("Mobile1", Mobile1), new XElement("Mobile2", Mobile2),
                new XElement("Mail1",Mail1), new XElement("Mail2", Mail2), new XElement("Facebook", Facebook), new XElement("Twitter", Twitter));
            return objectnode;
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("The given element is not named Contact and will not be parsed.");

            Phone1 = source.Element("Phone1").Value;
            Phone2 = source.Element("Phone2").Value;
            Fax1 = source.Element("Fax1").Value;
            Fax2 = source.Element("Fax2").Value;
            Mobile1 = source.Element("Mobile1").Value;
            Mobile2 = source.Element("Mobile2").Value;
            Mail1 = source.Element("Mail1").Value;
            Mail2 = source.Element("Mail2").Value;
            Facebook = source.Element("Facebook").Value;
            Twitter = source.Element("Twitter").Value;
        }

        public string XElementName
        {
            get { return "Contact"; }
        }

        public string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public string IDFieldName
        {
            get { return "ID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Contact();
        }
    }
}
