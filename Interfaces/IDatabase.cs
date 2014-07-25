using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biller.Core.Interfaces
{
    public interface IDatabase
    {
        #region Database
        /// <summary>
        /// Indicates if the database connection was established for the first time. In this case some files are missing.
        /// </summary>
        bool IsFirstLoad { get; }

        /// <summary>
        /// Indicates if the database connection was established correctly.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Awaitable function that connects a database in its specific implementation.
        /// </summary>
        /// <remarks>Returns false in a "first run" case but sets <see cref="IsFirstLoad"/> to "true" in this case.</remarks>
        /// <returns>Returns true if the attempt to connect was successfull.</returns>
        Task<bool> Connect();
        #endregion
        
        #region Company
        /// <summary>
        /// Contains information about the current used company.
        /// </summary>
        Models.CompanyInformation CurrentCompany { get; }

        /// <summary>
        /// Creates a database for a new company.
        /// </summary>
        /// <param name="source">Object that contains information about the new company.</param>
        void AddCompany(Models.CompanyInformation source);

        /// <summary>
        /// Changes the database. It will also call <see cref="Connect"/>. \n
        /// This function is awaitable.
        /// </summary>
        /// <param name="target">Object that contains information about the company you want switch to.</param>
        /// <returns>Returns a boolean whether the change was successfull.</returns>
        Task<bool> ChangeCompany(Models.CompanyInformation target);

        /// <summary>
        /// Awaitable function that returns a list of all registered companies.
        /// </summary>
        /// <returns>Returns a list of all companies the database can find.</returns>
        Task<IEnumerable<Models.CompanyInformation>> GetCompanyList();
        #endregion

        #region General Settings
        /// <summary>
        /// Awaitable function returing a list of all saved <see cref="Unit"/>s.
        /// </summary>
        /// <returns>Returns a list of <see cref="Unit"/>s.</returns>
        Task<IEnumerable<Utils.Unit>> ArticleUnits();

        /// <summary>
        /// Awaitable function returing a list of all saved <see cref="PaymentMethode"/>s.
        /// </summary>
        /// <returns>Returns a list of <see cref="PaymentMethode"/>s.</returns>
        Task<IEnumerable<Utils.PaymentMethode>> PaymentMethodes();

        /// <summary>
        /// Saves or updates an existing <see cref="Unit"/>.
        /// </summary>
        /// <param name="source">Object of type <see cref="Unit"/> containing input data.</param>
        void SaveOrUpdateArticleUnit(Utils.Unit source);

        /// <summary>
        /// Saves or updates an existing <see cref="PaymentMethode"/>.
        /// </summary>
        /// <param name="source">Object of type <see cref="PaymentMethode"/> containing input data.</param>
        void SaveOrUpdatePaymentMethode(Utils.PaymentMethode source);

        /// <summary>
        /// Saves or updates an existing <see cref="TaxClass"/>.
        /// </summary>
        /// <param name="source">Object of type <see cref="TaxClass"/> containing input data.</param>
        void SaveOrUpdateTaxClass(Utils.TaxClass source);

        /// <summary>
        /// Awaitable function returing a list of all saved <see cref="TaxClass"/>es.
        /// </summary>
        /// <returns>Returns a list of <see cref="TaxClass"/>es.</returns>
        Task<IEnumerable<Utils.TaxClass>> TaxClasses();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        void SaveOrUpdateSettings(Utils.KeyValueStore settings);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<Utils.KeyValueStore> GetSettings();
        #endregion

        #region Article
        /// <summary>
        /// Awaitable function returning all saved <see cref="Article"/>s as object of type <see cref="PreviewArticle"/>.
        /// </summary>
        /// <returns>Returns a list of <see cref="PreviewArticle"/>s.</returns>
        Task<IEnumerable<Articles.PreviewArticle>> AllArticles();

        /// <summary>
        /// Awaitable function checking if the database contains an <see cref="Article"/> with the specific ArticleID.
        /// </summary>
        /// <param name="ArticleID">Unique identifier of your <see cref="Article"/>.</param>
        /// <returns>Returns a boolean whether the article exists (true) or not (false).</returns>
        Task<bool> ArticleExists(string ArticleID);

        /// <summary>
        /// Awaitable function returning all saved data associated with the ArticleID.
        /// </summary>
        /// <param name="ArticleID">The unique identifier of your <see cref="Article"/>.</param>
        /// <returns>Returns object of type <see cref="Article"/>.</returns>
        Task<Articles.Article> GetArticle(string ArticleID);

        /// <summary>
        /// Awaitable function returning the next possible ArticleID as Integer.
        /// </summary>
        /// <returns>Returns the next higher value you can use as ArticleID. Returns "-1" if the current highest value is not an Integer.</returns>
        Task<int> GetNextArticleID();

        /// <summary>
        /// Saves or updates an existing article. 
        /// </summary>
        /// <param name="source">The source object containing the data you want to save.</param>
        Task<bool> SaveOrUpdateArticle(Articles.Article source);

        /// <summary>
        /// Saves or updates a list of <see cref="Article"/>s without saving the file after each item.
        /// </summary>
        /// <param name="source">The source object containing the data you want to save.</param>
        Task<bool> SaveOrUpdateArticle(IEnumerable<Articles.Article> source);

        /// <summary>
        /// Updates an ID the user is editing with but did not save it yet. This avoids double used IDs
        /// </summary>
        /// <param name="CustomerID"></param>
        Task<bool> UpdateTemporaryUsedArticleID(string oldvalue, string newvalue);
        #endregion

        #region Customer
        /// <summary>
        /// Awaitable function returning all saved <see cref="Customer"/>s as object of type <see cref="PreviewCustomer"/>.
        /// </summary>
        /// <returns>Returns a list of <see cref="PreviewCustomer"/>s.</returns>
        Task<IEnumerable<Customers.PreviewCustomer>>AllCustomers();

        /// <summary>
        /// Awaitable function checking if the database contains a <see cref="Customer"/> with the specific CustomerID.
        /// </summary>
        /// <param name="CustomerID">Unique identifier of your <see cref="Customer"/>.</param>
        /// <returns>Returns a boolean whether the <see cref="Customer"/> exists (true) or not (false).</returns>
        Task<bool> CustomerExists(string CustomerID);

        /// <summary>
        /// Awaitable function returning all saved data associated with the CustomerID.
        /// </summary>
        /// <param name="CustomerID">The unique identifier of your <see cref="Customer"/>.</param>
        /// <returns>Returns object of type <see cref="Customer"/>.</returns>
        Task<Customers.Customer> GetCustomer(string CustomerID);

        /// <summary>
        /// Awaitable function returning the next possible CustomerID as Integer.
        /// </summary>
        /// <returns>Returns the next higher value you can use as CustomerID. Returns "-1" if the current highest value is not an Integer.</returns>
        Task<int> GetNextCustomerID();

        /// <summary>
        /// Saves or updates an existing <see cref="Customer"/>. 
        /// </summary>
        /// <param name="source">The source object containing the data you want to save.</param>
        Task<bool> SaveOrUpdateCustomer(Customers.Customer source);

        /// <summary>
        /// Updates an ID the user is editing with but did not save it yet. This avoids double used IDs
        /// </summary>
        /// <param name="oldvalue"></param>
        /// <param name="newvalue"></param>
        Task<bool> UpdateTemporaryUsedCustomerID(string oldvalue, string newvalue);

        /// <summary>
        /// Saves or updates a list of <see cref="Customer"/>s without saving the file after a new item.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<bool> SaveOrUpdateCustomer(IEnumerable<Customers.Customer> source);
        #endregion

        #region Document
        /// <summary>
        /// Awaitable function returning all saved <see cref="Document"/>s. Object types may vary in object type but inheriting all from <see cref="Document"/>.
        /// </summary>
        /// <param name="IntervalStart">Beginning of the interval.</param>
        /// <param name="IntervalEnd">End of the interval.</param>
        /// <returns>Returns a list of <see cref="PreviewCustomer"/>s.</returns>
        Task<IEnumerable<Document.PreviewDocument>> DocumentsInInterval(DateTime IntervalStart, DateTime IntervalEnd);

        /// <summary>
        /// Awaitable function returning all saved <see cref="Document"/>s matching the input type.
        /// </summary>
        /// <param name="IntervalStart">Beginning of the interval.</param>
        /// <param name="IntervalEnd">End of the interval.</param>
        /// <param name="DocumentType">Specific implementation of the <see cref="Document"/></param>
        /// <returns>Returns a list of <see cref="PreviewCustomer"/>s.</returns>
        Task<IEnumerable<Document.PreviewDocument>> DocumentsInInterval(DateTime IntervalStart, DateTime IntervalEnd, string DocumentType);
        
        /// <summary>
        /// Awaitable function checking if the database contains a <see cref="Document"/> with the specific ID and type.
        /// </summary>
        /// <param name="source.DocumentID">Unique identifier of your <see cref="Document"/>.</param>
        /// <param name="source.DocumentType">Type of your <see cref="Document"/> as string.</param>
        /// <returns>Returns a boolean whether the <see cref="Document"/> exists (true) or not (false).</returns>
        Task<bool> DocumentExists(Document.Document source);

        /// <summary>
        /// Awaitable function returning the next possible DocumentID as Integer.
        /// </summary>
        /// param name="DocumentType">Type of your <see cref="Document"/> as string.</param>
        /// <returns>Returns the next higher value you can use as DocumentID. Returns "-1" if the highest value is not an Integer.</returns>
        Task<int> GetNextDocumentID(string DocumentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source.DocumentID">Unique identifier of your <see cref="Document"/>.</param>
        /// <param name="source.DocumentType">Type of your <see cref="Document"/> as string.</param>
        /// <returns></returns>
        Task<Document.Document> GetDocument(Document.Document source);

        /// <summary>
        /// Saves or updates an existing object of <see cref="Document"/>.
        /// </summary>
        /// <param name="source">The object you want to save.</param>
        Task<bool> SaveOrUpdateDocument(Document.Document source);

        /// <summary>
        /// Updates an ID the user is editing with but did not save it yet. This avoids double used IDs
        /// </summary>
        /// <param name="oldvalue"></param>
        /// <param name="newvalue"></param>
        Task<bool> UpdateTemporaryUsedDocumentID(string oldID, string newID, string DocumentType);

        /// <summary>
        /// Allows to implement an additional parser to add your specific data to an <see cref="PreviewDocument"/>
        /// </summary>
        /// <returns></returns>
        Task<bool> AddAdditionalPreviewDocumentParser(Interfaces.DocumentParser parser);
        #endregion

        #region IXMLStorageable
        Task<IEnumerable<IXMLStorageable>> AllStorageableItems(IXMLStorageable referenceStorageable);

        Task<bool> StorageableItemExists(IXMLStorageable referenceStorageable);

        Task<bool> SaveOrUpdateStorageableItem(IXMLStorageable StorageableItem);

        Task<bool> RegisterStorageableItem(IXMLStorageable StorageableItem);
        #endregion
    }
}