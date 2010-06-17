using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DigitalFrame.Core.Interfaces;

namespace DigitalFrame.Core
{
    public abstract class Repository<T> : IRepository<T>
    {
        protected const string ROOT_ELEMENT_NAME = "settings";

        #region IRepository<T> Members

        public abstract T Load();

        public abstract void Save(T value);

        #endregion

        protected static string GetSettingValue(XDocument xDocument, string key)
        {
            string settingValue = (from s in xDocument.Descendants("setting")
                                   where s.Attribute("key").Value == key
                                   select s.Attribute("value").Value).SingleOrDefault();

            return settingValue;
        }

        protected static XDocument LoadDocument(string fileName)
        {
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (
                    var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate,
                                                                                  FileAccess.Read, storageFile))
                {
                    using (XmlReader xmlReader = XmlReader.Create(isolatedStorageFileStream))
                    {
                        try
                        {
                            return XDocument.Load(xmlReader);
                        }
                        catch (XmlException)
                        {
                            return null;
                        }
                    }
                }
            }
        }

        protected static void WriteDocument(string fileName, XDocument xDocument)
        {
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (
                    var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate,
                                                                                  FileAccess.ReadWrite, storageFile))
                {
                    isolatedStorageFileStream.SetLength(0);

                    using (XmlWriter xmlWriter = XmlWriter.Create(isolatedStorageFileStream))
                    {
                        xDocument.Save(xmlWriter);
                    }
                }
            }
        }

        protected static XDocument CreateDocumentWithRoot(string rootElementName)
        {
            var xDocument = new XDocument();
            xDocument.Add(new XElement(rootElementName));

            return xDocument;
        }

        protected static XElement CreateSettingElement(string key, string value)
        {
            var settingElement = new XElement("setting");

            settingElement.Add(new XAttribute("key", key));
            settingElement.Add(new XAttribute("value", value));

            return settingElement;
        }
    }
}