using LibXbf.Records;
using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using System.Collections.Generic;
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

        public List<XbfNode> Nodes { get; set; }

        public XbfFile(string path)
        {
            Nodes = new List<XbfNode>();

            using (FileStream fStr = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fStr, Encoding.Unicode))
            {
                // read the file header
                Header = new XbfHeader(br);

                //read in the tables
                StringTable = new XbfTable<string, XbfString>(br);
                AssemblyTable = new XbfTable<AssemblyEntry, XbfAssemblyEntry>(br);
                TypeNamespaceTable = new XbfTable<TypeNamespaceEntry, XbfTypeNamespaceEntry>(br);
                TypeTable = new XbfTable<TypeEntry, XbfTypeEntry>(br);
                PropertyTable = new XbfTable<PropertyEntry, XbfPropertyEntry>(br);
                XmlNamespaceTable = new XbfTable<uint, XbfUInt32>(br);

                // read in the nodes
                ReadNodes(br);
            }
        }

        private void ReadNodes(BinaryReader br)
        {
            XbfLineInfo currentLineInfo = new XbfLineInfo()
            {
                LineNumber = 0,
                LinePosition = 0
            };

            // check if there's still more to read
            while (br.BaseStream.Length != br.BaseStream.Position)
            {
                XbfNodeType type = (XbfNodeType)br.ReadByte();

                switch (type)
                {
                    case XbfNodeType.StartObject:
                        {
                            XbfObject xo = new XbfObject(br);
                            xo.LineInfo = currentLineInfo;
                            Nodes.Add(xo);
                            continue;
                        }
                    case XbfNodeType.EndObject:
                        continue;
                    case XbfNodeType.StartProperty:
                        {
                            XbfProperty xp = new XbfProperty(br);
                            xp.LineInfo = currentLineInfo;
                            Nodes.Add(xp);
                            continue;
                        }
                    case XbfNodeType.EndProperty:
                        continue;
                    case XbfNodeType.Text:
                        {
                            XbfText xt = new XbfText(br);
                            xt.LineInfo = currentLineInfo;
                            Nodes.Add(xt);
                            continue;
                        }
                    case XbfNodeType.Value:
                        {
                            XbfValue xv = new XbfValue(br);
                            xv.LineInfo = currentLineInfo;
                            Nodes.Add(xv);
                            continue;
                        }
                    case XbfNodeType.Namespace:
                        {
                            XbfNamespace xn = new XbfNamespace(br);
                            xn.LineInfo = currentLineInfo;
                            Nodes.Add(xn);
                            continue;
                        }
                    case XbfNodeType.LineInfo:
                        {
                            XbfLineInfo newLineInfo = new XbfLineInfo()
                            {
                                LineNumber = (uint)(currentLineInfo.LineNumber + br.ReadInt16()),
                                LinePosition = (uint)(currentLineInfo.LinePosition + br.ReadInt16())
                            };

                            currentLineInfo = newLineInfo;
                            continue;
                        }
                    case XbfNodeType.LineInfoAbsolute:
                        {
                            currentLineInfo = new XbfLineInfo()
                            {
                                LineNumber = br.ReadUInt32(),
                                LinePosition = br.ReadUInt32()
                            };
                            continue;
                        }
                    case XbfNodeType.EndOfAttributes:
                    case XbfNodeType.EndOfStream:
                    default:
                        continue;
                }
            }
        }
    }
}
