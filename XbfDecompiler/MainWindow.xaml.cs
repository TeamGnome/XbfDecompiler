using LibXbf;
using LibXbf.Output;
using LibXbf.Records.Nodes;
using LibXbf.Records.Types;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;

namespace XbfDecompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XbfFile currentFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XAML Binary Files|*.xbf";
            ofd.CheckFileExists = true;
            ofd.AddExtension = true;

            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                currentFile = new XbfFile(ofd.FileName);

                TreeOutput to = new TreeOutput();
                var treeout = to.GetOutput(currentFile);
                xbfTree.ItemsSource = new[] { treeout };

                XamlOutput xo = new XamlOutput();
                var output = xo.GetOutput(currentFile);
                using (MemoryStream ms = new MemoryStream())
                {
                    // need these extra settings to clean up the splurge of namespaces
                    using (XmlWriter xw = XmlWriter.Create(ms, new XmlWriterSettings()
                    {
                        OmitXmlDeclaration = true,
                        NamespaceHandling = NamespaceHandling.OmitDuplicates,
                        Indent = true,
                        NewLineHandling = NewLineHandling.Entitize,
                        Encoding = Encoding.UTF8
                    }))
                    {
                        output.WriteTo(xw);
                    }

                    // reset to start for reader
                    ms.Position = 0;

                    using (TextReader tr = new StreamReader(ms))
                    {
                        xbfXml.Text = tr.ReadToEnd();
                    }
                }
            }
        }
    }
}
