using LibXbf.Records;
using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibXbf
{
    public class XbfFile
    {
        private static readonly byte[] fileMagic = new byte[] { 0x58, 0x42, 0x46, 0x00 };

        public Version FileVersion { get; set; }
        public XbfHeader Header { get; set; }
        public XbfTable<string, XbfString> StringTable { get; set; }
        public XbfTable<AssemblyEntry, XbfAssemblyEntry> AssemblyTable { get; set; }
        public XbfTable<TypeNamespaceEntry, XbfTypeNamespaceEntry> TypeNamespaceTable { get; set; }
        public XbfTable<TypeEntry, XbfTypeEntry> TypeTable { get; set; }
        public XbfTable<PropertyEntry, XbfPropertyEntry> PropertyTable { get; set; }
        public XbfTable<uint, XbfUInt32> XmlNamespaceTable { get; set; }

        public XbfObject RootNode { get; set; }
        public List<XbfNamespace> Namespaces { get; set; }

        public XbfFile(string path)
        {
            Namespaces = new List<XbfNamespace>();

            using (FileStream fStr = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fStr, Encoding.Unicode))
            {
                // read the file header
                Header = new XbfHeader(br);

                if(!verifyMagic())
                {
                    throw new InvalidXbfException();
                }

                FileVersion = new Version((int)Header.MajorFileVersion, (int)Header.MinorFileVersion);

                //read in the tables
                StringTable = new XbfTable<string, XbfString>(br, FileVersion);
                AssemblyTable = new XbfTable<AssemblyEntry, XbfAssemblyEntry>(br, FileVersion);
                TypeNamespaceTable = new XbfTable<TypeNamespaceEntry, XbfTypeNamespaceEntry>(br, FileVersion);
                TypeTable = new XbfTable<TypeEntry, XbfTypeEntry>(br, FileVersion);
                PropertyTable = new XbfTable<PropertyEntry, XbfPropertyEntry>(br, FileVersion);
                XmlNamespaceTable = new XbfTable<uint, XbfUInt32>(br, FileVersion);

                // read in the nodes
                readNodes(br);
            }
        }

        private bool verifyMagic()
        {
            bool ret = true;

            for(int i = 0; i < fileMagic.Length; i++)
            {
                ret &= (fileMagic[i] == Header.MagicNumber[i]);
            }

            return ret;
        }

        private void readNodes(BinaryReader br)
        {
            Stack<XbfObject> objectStack = new Stack<XbfObject>();
            Stack<XbfProperty> propertyStack = new Stack<XbfProperty>();

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

                            // Is this the first Object?
                            if(RootNode == null)
                            {
                                // Then it's the root node.
                                RootNode = xo;
                            }

                            // Push it to the object stack
                            objectStack.Push(xo);
                            continue;
                        }
                    case XbfNodeType.EndObject:
                        {
                            // are we within a property?
                            if (propertyStack.Count > 0)
                            {
                                // set the current object as the property's value
                                propertyStack.Peek().Values.Add(objectStack.Peek());
                            }

                            // Current object is finished with, pop it from the stack
                            objectStack.Pop();
                            continue;
                        }
                    case XbfNodeType.StartProperty:
                        {
                            XbfProperty xp = new XbfProperty(br);
                            xp.LineInfo = currentLineInfo;

                            // Push it to the property stack
                            propertyStack.Push(xp);
                            continue;
                        }
                    case XbfNodeType.EndProperty:
                        {
                            // are we within an object? To be honest, I think this should probably always be true for a valid XBF file, but check all the same.
                            if(objectStack.Count > 0)
                            {
                                // add the property to the current object
                                objectStack.Peek().Properties.Add(propertyStack.Peek());
                            }

                            // Current property is complete, pop it from the stack
                            propertyStack.Pop();
                            continue;
                        }
                    case XbfNodeType.Text:
                        {
                            XbfText xt = new XbfText(br);
                            xt.LineInfo = currentLineInfo;

                            // are we currently inside a property? should probably always be true I think
                            if(propertyStack.Count > 0)
                            {
                                // add it as a value for the current property
                                propertyStack.Peek().Values.Add(xt);
                            }
                            continue;
                        }
                    case XbfNodeType.Value:
                        {
                            XbfValue xv = new XbfValue(br);
                            xv.LineInfo = currentLineInfo;

                            // are we currently inside a property? should probably always be true I think
                            if (propertyStack.Count > 0)
                            {
                                // add it as a value for the current property
                                propertyStack.Peek().Values.Add(xv);
                            }
                            continue;
                        }
                    case XbfNodeType.Namespace:
                        {
                            XbfNamespace xn = new XbfNamespace(br);
                            xn.LineInfo = currentLineInfo;

                            // These don't get added in the normal hierarchy.
                            Namespaces.Add(xn);
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
