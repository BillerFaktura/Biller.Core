using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Customers
{
    /// <summary>
    /// Dynamic class to preview data on the UI.
    /// <example>Use this class as shown below to save your own data to an instance of this object.
    /// <code>
    /// <pre>
    /// dynamic previewCustomer = new Data.Customers.CustomerArticle();
    /// previewCustomer.YourPropertyName = "YourPropertyValue";
    /// </pre>
    /// </code>
    /// </example>
    /// </summary>
    public class PreviewCustomer : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for <see cref="PreviewCustomer"/>. Initializes all internal objects and defines empty strings for any predefined property.
        /// </summary>
        public PreviewCustomer()
        {
            propertyValueStorage = new Dictionary<string, object>();
            CustomerID = "";
        }

        public string CustomerID
        {
            get { return GetValue(() => CustomerID); }
            set { SetValue(value); }
        }

        public static PreviewCustomer FromCustomer(Customer source)
        {
            dynamic temp = new PreviewCustomer() { CustomerID = source.CustomerID };
            temp.DisplayName = source.DisplayName;
            temp.Address = source.MainAddress.OneLineString;
            return temp;
        }

        public override bool Equals(object obj)
        {
            if (obj is PreviewCustomer)
                if (CustomerID == (obj as PreviewCustomer).CustomerID)
                    return true;
            return false;
        }

        #region PropertyHelper

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return propertyValueStorage.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetValue(value, binder.Name);
            return true;
        }

        public Dictionary<string, object> propertyValueStorage { get; private set; }
        /// <summary>
        /// Get the value of the property
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="property">The property as a lambda expression</param>
        /// <returns>The value of the given property (or the default value)</returns>
        public T GetValue<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = property as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Invalid lambda expression", "Lambda expression return value can't be null");
            }

            string propertyName = this.getPropertyName(lambdaExpression);

            return getValue<T>(propertyName);
        }

        /// <summary>
        /// Try to get the value from the internal dictionary of the given property name
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>Retrieve the value from the internal dictionary</returns>
        private T getValue<T>(string propertyName)
        {
            object value;

            if (propertyValueStorage.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Extract the property name from a lambda expression
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression with the property</param>
        /// <returns>The extracted property name</returns>
        private string getPropertyName(LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;

            if (lambdaExpression.Body is UnaryExpression)
            {
                var unaryExpression = lambdaExpression.Body as UnaryExpression;

                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Saves the value of an object in a dictionary and automatically raises a PropertyChangedEvent if the value differs.
        /// </summary>
        /// <typeparam name="T">Your object type</typeparam>
        /// <param name="value">Instance of your object</param>
        /// <param name="name">The name of the calling member. You don't need to insert a value here - leave it empty.</param>
        public void SetValue<T>(T value, [CallerMemberName] string name = "")
        {
            if (!EqualityComparer<T>.Default.Equals(getValue<T>(name), value))
            {
                propertyValueStorage[name] = value;
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }
}
