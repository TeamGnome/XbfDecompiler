using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LibXbf.Output
{
    public class XamlOutput : IXbfOutput<XElement>
    {
        public XbfFile CurrentFile { get; private set; }

        public XElement GetOutput(XbfFile file)
        {
            CurrentFile = file;

            List<XAttribute> nsDeclarations = new List<XAttribute>();
            foreach(var ns in CurrentFile.XmlNamespaceTable.Values)
            {
                var xns = XNamespace.Get(CurrentFile.StringTable.Values[ns]);
                string shortName = GetXmlnsAttributeNameForUrl(xns.NamespaceName);
                nsDeclarations.Add(new XAttribute(string.IsNullOrEmpty(shortName) ? "xmlns" : XNamespace.Xmlns + shortName, xns.NamespaceName));
            }

            var rootObj = CurrentFile.TypeTable.Values[CurrentFile.RootNode.Id];
            string rootName = CurrentFile.StringTable.Values[rootObj.StringId];
            var rootElement = new XElement(XName.Get(rootName, "http://schemas.microsoft.com/winfx/2006/xaml/presentation"));
            rootElement.Add(nsDeclarations.ToArray());

            var properties = from x in CurrentFile.RootNode.Properties
                             where x is XbfProperty
                             select DumpXbfPropertyToXml(x as XbfProperty);
            rootElement.Add(properties);

            return rootElement;
        }

        private XElement DumpXbfObjectToXml(XbfObject xo)
        {
            var obj = CurrentFile.TypeTable.Values[xo.Id];
            string disp = CurrentFile.StringTable.Values[obj.StringId];

            XElement xe = new XElement(disp);

            var properties = from x in xo.Properties
                             where x is XbfProperty
                             select DumpXbfPropertyToXml(x as XbfProperty);
            xe.Add(properties);

            return xe;
        }

        private XObject DumpXbfPropertyToXml(XbfProperty xp)
        {
            var property = CurrentFile.PropertyTable.Values[xp.Id];
            XName disp = property.Flags.HasFlag(PropertyFlags.IsMarkupDirective) ?
                            XName.Get(CurrentFile.StringTable.Values[property.StringId], "http://schemas.microsoft.com/winfx/2006/xaml") :
                            CurrentFile.StringTable.Values[property.StringId];

            var objects = from x in xp.Values
                          where x is XbfObject
                          select DumpXbfObjectToXml(x as XbfObject);

            if (objects.Count() > 0)
            {
                XElement xe = new XElement(disp);
                xe.Add(objects);
                return xe;
            }
            else
            {
                XAttribute xa = new XAttribute(disp, "");
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
                return xa;
            }
        }

        private string DumpXbfTextToXml(XbfText xt)
        {
            string disp = CurrentFile.StringTable.Values[xt.Id];

            return disp;
        }

        private string DumpXbfValueToXml(XbfValue xv)
        {
            return xv.Value;
        }

        private string GetXmlnsAttributeNameForUrl(string nsUrl)
        {
            string nsUrlLower = nsUrl.ToLower();
            switch(nsUrlLower)
            {
                case "http://schemas.microsoft.com/winfx/2006/xaml/presentation":
                    return "";
                case "http://schemas.microsoft.com/winfx/2006/xaml":
                    return "x";
                case "http://schemas.microsoft.com/expression/blend/2008":
                    return "d";
                case "http://schemas.openxmlformats.org/markup-compatibility/2006":
                    return "mc";
                default:
                    {
                        if(nsUrlLower.StartsWith("using:") || nsUrlLower.StartsWith("clr-namespace:"))
                        {
                            return nsUrlLower.Substring(nsUrlLower.LastIndexOf('.') + 1);
                        }
                        else
                        {
                            return "xml" + nsUrlLower.GetHashCode().ToString("X8").ToLower();
                        }
                    }
            }
        }
    }
}
