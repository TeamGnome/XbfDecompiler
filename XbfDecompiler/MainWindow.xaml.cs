using LibXbf;
using LibXbf.Records.Nodes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XbfDecompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class XbfTreeItem
        {
            public string Display { get; set; }
            public IEnumerable<XbfTreeItem> Children { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XAML Binary Files|*.xbf";
            ofd.CheckFileExists = true;
            ofd.AddExtension = true;

            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                XbfFile xr = new XbfFile(ofd.FileName);

                List<XbfTreeItem> source = new List<XbfTreeItem>();
                source.Add(DumpXbfObject(xr.RootNode));
                xbfTree.ItemsSource = source;
            }
        }

        private XbfTreeItem DumpXbfObject(XbfObject xo)
        {
            return new XbfTreeItem()
            {
                Display = "XbfObject",
                Children = from prop in xo.Properties
                           select DumpXbfProperty(prop)
            };
        }

        private XbfTreeItem DumpXbfProperty(XbfProperty xp)
        {
            return new XbfTreeItem()
            {
                Display = "XbfProperty",
                Children = from node in xp.Values
                           select DumpXbfNode(node)
            };
        }

        private XbfTreeItem DumpXbfText(XbfText xt)
        {
            return new XbfTreeItem()
            {
                Display = "XbfText",
                Children = null
            };
        }

        private XbfTreeItem DumpXbfValue(XbfValue xv)
        {
            return new XbfTreeItem()
            {
                Display = string.Format("VALUE: {0}", xv.Value),
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
