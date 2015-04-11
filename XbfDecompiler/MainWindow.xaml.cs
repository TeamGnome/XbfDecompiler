using LibXbf;
using LibXbf.Output;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
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
                xbfXml.Text = output.ToString(SaveOptions.OmitDuplicateNamespaces);
            }
        }
    }
}
