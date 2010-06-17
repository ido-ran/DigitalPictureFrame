using System.Xml.Linq;
using DigitalFrame.Core;

namespace DigitalFrame.Service.FileSystemImage
{
    public class FileSystemImageSettingsRepository : Repository<FileSystemImageSettings>
    {
        private const string FILE_NAME = "FileSystemImageSettings.xml";
        private static readonly FileSystemImageSettings DefaultFileSystemImageSettings = new FileSystemImageSettings { Path = @"C:\Documents and Settings\All Users\Documents\My Pictures\Sample Pictures" };

        public override FileSystemImageSettings Load()
        {
            XDocument document = LoadDocument(FILE_NAME);

            if (document == null)
            {
                return DefaultFileSystemImageSettings;
            }

            string path = GetSettingValue(document, "Path");

            return new FileSystemImageSettings { Path = path };
        }

        public override void Save(FileSystemImageSettings fileSystemImageSettings)
        {
            XDocument xDocument = CreateDocumentWithRoot(ROOT_ELEMENT_NAME);
            XElement pathSetting = CreateSettingElement("Path", fileSystemImageSettings.Path);
            xDocument.Element(ROOT_ELEMENT_NAME).Add(pathSetting);

            WriteDocument(FILE_NAME, xDocument);
        }
    }
}