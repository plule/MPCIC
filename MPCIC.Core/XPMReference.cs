using System.IO;
using System.Reflection;
using System.Xml;

namespace MPCIC.Core
{
    public class XPMReference
    {
        public static XmlDocument GetXPMReference()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            Stream resourceStream = assembly.GetManifestResourceStream("MPCIC.Core.Reference.xpm");
            XmlDocument doc = new XmlDocument();
            doc.Load(resourceStream);
            return doc;
        }
    }
}
