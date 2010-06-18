using System.Xml.Linq;
using DigitalFrame.Core;

namespace DigitalFrame.Service.PicasaImage
{
    public class PicasaImageSettingsRepository : Repository<PicasaImageSettings>
    {
        private const string FILE_NAME = "PicasaImageSettings.xml";
        private static readonly PicasaImageSettings DefaultPicasaImageSettings = new PicasaImageSettings { Username = "", Password = "" };

        public override PicasaImageSettings Load()
        {
            XDocument document = LoadDocument(FILE_NAME);

            if (document == null)
            {
                return DefaultPicasaImageSettings;
            }

            string username = GetSettingValue(document, "Username");
            string password = GetSettingValue(document, "Password");

            return new PicasaImageSettings()
            {
              Username = username,
              Password = password
            };
        }

        public override void Save(PicasaImageSettings settings)
        {
            XDocument xDocument = CreateDocumentWithRoot(ROOT_ELEMENT_NAME);
            XElement usernameElement = CreateSettingElement("Username", settings.Username);
            XElement passwordElement = CreateSettingElement("Password", settings.Password);
            xDocument.Element(ROOT_ELEMENT_NAME).Add(usernameElement);
            xDocument.Element(ROOT_ELEMENT_NAME).Add(passwordElement);

            WriteDocument(FILE_NAME, xDocument);
        }
    }
}