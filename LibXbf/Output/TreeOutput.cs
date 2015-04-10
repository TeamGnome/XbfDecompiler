using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using System.Linq;

namespace LibXbf.Output
{
    public class XbfTreeItem
    {
        public string Display { get; set; }
        public XbfTreeItem[] Children { get; set; }
    }

    public class TreeOutput : IXbfOutput<XbfTreeItem>
    {
        public XbfFile CurrentFile { get; private set; }

        public XbfTreeItem GetOutput(XbfFile file)
        {
            CurrentFile = file;

            return DumpXbfObject(CurrentFile.RootNode);
        }

        private XbfTreeItem DumpXbfObject(XbfObject xo)
        {
            var obj = CurrentFile.TypeTable.Values[xo.Id];
            string disp = CurrentFile.StringTable.Values[obj.StringId];

            return new XbfTreeItem()
            {
                Display = disp,
                Children = (from prop in xo.Properties
                           select DumpXbfProperty(prop)).ToArray()
            };
        }

        private XbfTreeItem DumpXbfProperty(XbfProperty xp)
        {
            var property = CurrentFile.PropertyTable.Values[xp.Id];
            string disp = property.Flags.HasFlag(PropertyFlags.IsMarkupDirective) ?
                            string.Format("x:{0}", CurrentFile.StringTable.Values[property.StringId]) :
                            CurrentFile.StringTable.Values[property.StringId];

            return new XbfTreeItem()
            {
                Display = disp,
                Children = (from node in xp.Values
                           select DumpXbfNode(node)).ToArray()
            };
        }

        private XbfTreeItem DumpXbfText(XbfText xt)
        {
            string disp = CurrentFile.StringTable.Values[xt.Id];

            return new XbfTreeItem()
            {
                Display = disp,
                Children = null
            };
        }

        private XbfTreeItem DumpXbfValue(XbfValue xv)
        {
            return new XbfTreeItem()
            {
                Display = string.Format("{0}: {1}", xv.Type, xv.Value),
                Children = null
            };
        }

        private XbfTreeItem DumpXbfNode(XbfNode xn)
        {
            if (xn is XbfObject)
            {
                return DumpXbfObject(xn as XbfObject);
            }
            else if (xn is XbfValue)
            {
                return DumpXbfValue(xn as XbfValue);
            }
            else if (xn is XbfText)
            {
                return DumpXbfText(xn as XbfText);
            }
            else
            {
                return null;
            }
        }
    }
}
