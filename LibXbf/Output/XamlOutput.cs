using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LibXbf.Output
{
    public class XamlOutput : IXbfOutput<XDocument>
    {
        public XbfFile CurrentFile { get; private set; }
        private List<string> declaredNamespaces { get; set; }

        public XDocument GetOutput(XbfFile file)
        {
            CurrentFile = file;
            declaredNamespaces = new List<string>();

            List<XAttribute> nsDeclarations = new List<XAttribute>();
            foreach (var ns in CurrentFile.Namespaces)
            {
                var xns = CurrentFile.StringTable.Values[CurrentFile.XmlNamespaceTable.Values[ns.Id]];
                nsDeclarations.Add(new XAttribute(string.IsNullOrEmpty(ns.Namespace) ? "xmlns" : XNamespace.Xmlns + ns.Namespace, xns));
                declaredNamespaces.Add(xns);
            }

            var rootObj = CurrentFile.TypeTable.Values[CurrentFile.RootNode.Id];
            XName rootName = GetXNameForObject(CurrentFile.StringTable.Values[rootObj.StringId], CurrentFile.StringTable.Values[CurrentFile.TypeNamespaceTable.Values[rootObj.NamespaceId].StringId]);
            var rootElement = new XElement(rootName);
            rootElement.Add(nsDeclarations.ToArray());

            var properties = from x in CurrentFile.RootNode.Properties
                             where x is XbfProperty
                             select DumpXbfPropertyToXml(x as XbfProperty);
            rootElement.Add(properties);
            var rootDoc = new XDocument(rootElement);

            return rootDoc;
        }

        private XElement DumpXbfObjectToXml(XbfObject xo)
        {
            var obj = CurrentFile.TypeTable.Values[xo.Id];
            XName disp = GetXNameForObject(CurrentFile.StringTable.Values[obj.StringId], CurrentFile.StringTable.Values[CurrentFile.TypeNamespaceTable.Values[obj.NamespaceId].StringId]);

            XElement xe = new XElement(disp);

            var properties = from x in xo.Properties
                             where x is XbfProperty
                             select DumpXbfPropertyToXml(x as XbfProperty);
            xe.Add(properties);

            return xe;
        }

        private object[] DumpXbfPropertyToXml(XbfProperty xp)
        {
            var property = CurrentFile.PropertyTable.Values[xp.Id];
            var isMarkupProperty = property.Flags.HasFlag(PropertyFlags.IsMarkupDirective);
            XName disp = GetXNameForObject(CurrentFile.StringTable.Values[property.StringId], isMarkupProperty ? "x" : CurrentFile.StringTable.Values[CurrentFile.TypeNamespaceTable.Values[CurrentFile.TypeTable.Values[property.TypeId].NamespaceId].StringId]);

            var objects = from x in xp.Values
                          where x is XbfObject
                          select DumpXbfObjectToXml(x as XbfObject);

            if (objects.Count() > 0)
            {
                if (disp.LocalName == "implicititems")
                {
                    return objects.ToArray();
                }
                else
                {
                    XElement xe = new XElement(disp);
                    xe.Add(objects);
                    return new XObject[] { xe };
                }
            }
            else if(disp.LocalName == "implicitinitialization")
            {
                var xText = xp.Values.FirstOrDefault(x => x is XbfText);

                return new object[] { DumpXbfTextToXml(xText as XbfText) };
            }
            else
            {
                // technically wrong, and may cause issues in the future, but fuck Microsoft's need to declare namespaces over and over and over and over and over and over and over and over. and over.
                XAttribute xa = new XAttribute(XName.Get(disp.LocalName, isMarkupProperty ? disp.NamespaceName : ""), "");
                var xText = xp.Values.FirstOrDefault(x => x is XbfText);
                if (xText != null)
                {
                    xa.SetValue(DumpXbfTextToXml(xText as XbfText));
                }
                var xVal = xp.Values.FirstOrDefault(x => x is XbfValue);
                if (xVal != null)
                {
                    xa.SetValue(DumpXbfValueToXml(xVal as XbfValue));
                }
                return new XObject[] { xa };
            }
        }

        private string DumpXbfTextToXml(XbfText xt)
        {
            return CurrentFile.StringTable.Values[xt.Id];
        }

        private string DumpXbfValueToXml(XbfValue xv)
        {
            return xv.Value;
        }

        private XName GetXNameForObject(string type, string typeNamespace)
        {
            // filter out any invalid characters
            string dispType = Regex.Replace(type, @"[^\p{L}\p{N}]+", "");

            if(typeNamespace.StartsWith("Windows.UI"))
            {
                return XName.Get(dispType, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            }
            else if(typeNamespace == "x" || typeNamespace == "Windows.Foundation")
            {
                return XName.Get(dispType, "http://schemas.microsoft.com/winfx/2006/xaml");
            }
            else
            {
                return XName.Get(dispType, string.Format("using:{0}", typeNamespace));
            }
        }
    }
}
