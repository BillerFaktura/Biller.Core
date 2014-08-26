using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Core.Articles
{
    /// <summary>
    /// Dynamic class to preview data on the UI.
    /// <example>Use this class as shown below to save your own data to an instance of this object.
    /// <code>
    /// <pre>
    /// dynamic previewArticle = new Data.Articles.PreviewArticle();
    /// previewArticle.YourPropertyName = "YourPropertyValue";
    /// </pre>
    /// </code>
    /// </example>
    /// </summary>
    public class PreviewArticle : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for <see cref="PreviewArticle"/>. Initializes all internal objects and sets the predefinied properties to an empty string or creates a new instance of a propertie's object.
        /// </summary>
        public PreviewArticle()
        {
            propertyValueStorage = new Dictionary<string, object>();
            ArticleID = "";
            ArticleDescription = "";
            ArticlePrice = new Utils.EMoney(0);
        }

        /// <summary>
        /// Predefined property. <see cref="Equals"/> uses this property to compare two objects.
        /// </summary>
        public string ArticleID
        {
            get { return GetValue(() => ArticleID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Predefined property.
        /// </summary>
        public string ArticleDescription
        {
            get { return GetValue(() => ArticleDescription); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Predefined property
        /// </summary>
        public string ArticleCategory
        {
            get { return GetValue(() => ArticleCategory); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Predefined property. Use <see cref="Money.AmountString"/> to show this objects value on a UI (this parses a string as well).
        /// </summary>
        public Utils.EMoney ArticlePrice
        {
            get { return GetValue(() => ArticlePrice); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Comapres two objects and returns whether they are equal or not.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is PreviewArticle)
                if (ArticleID == (obj as PreviewArticle).ArticleID)
                    return true;
            return false;
        }

        /// <summary>
        /// Creates a <see cref="PreviewArticle"/> from a <see cref="Article"/>.
        /// </summary>
        /// <param name="source">Object of the type <see cref="Article"/>.</param>
        /// <returns>Returns a <see cref="PreviewArticle"/> where the properties ArticleID, ArticleDescription, ArticleCategory, ArticlePrice and ArticleUnit are set.</returns>
        public static PreviewArticle FromArticle(Article source)
        {
            dynamic temp = new PreviewArticle() { ArticleID = source.ArticleID, ArticleDescription = source.ArticleDescription, ArticleCategory = source.ArticleCategory, ArticlePrice = source.Price1.Price1 };
            temp.ArticleUnit = source.ArticleUnit.Name;
            return temp;
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

        private Dictionary<string, object> propertyValueStorage { get; set; }
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
