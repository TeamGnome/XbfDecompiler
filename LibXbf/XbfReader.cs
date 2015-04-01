using LibXbf.Records;
using System.IO;
using System.Text;

namespace LibXbf
{
    public class XbfFile
    {
        public XbfHeader Header { get; set; }
        public XbfTable<string, XbfString> StringTable { get; set; }
        public XbfTable<AssemblyEntry, XbfAssemblyEntry> AssemblyTable { get; set; }
        public XbfTable<TypeNamespaceEntry, XbfTypeNamespaceEntry> TypeNamespaceTable { get; set; }
        public XbfTable<TypeEntry, XbfTypeEntry> TypeTable { get; set; }
        public XbfTable<PropertyEntry, XbfPropertyEntry> PropertyTable { get; set; }
        public XbfTable<uint, XbfUInt32> XmlNamespaceTable { get; set; }

        public XbfFile(string path)
        {
            using (FileStream fStr = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fStr, Encoding.Unicode))
            {
                Header = new XbfHeader(br);
                StringTable = new XbfTable<string, XbfString>(br);
                AssemblyTable = new XbfTable<AssemblyEntry, XbfAssemblyEntry>(br);
                TypeNamespaceTable = new XbfTable<TypeNamespaceEntry, XbfTypeNamespaceEntry>(br);
                TypeTable = new XbfTable<TypeEntry, XbfTypeEntry>(br);
                PropertyTable = new XbfTable<PropertyEntry, XbfPropertyEntry>(br);
                XmlNamespaceTable = new XbfTable<uint, XbfUInt32>(br);
            }
        }
    }
}
