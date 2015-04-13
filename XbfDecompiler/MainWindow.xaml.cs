using LibXbf;
using LibXbf.Output;
using Microsoft.Win32;
using System;
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
                try
                {
                    currentFile = new XbfFile(ofd.FileName);
                }
                catch(InvalidXbfException)
                {
                    MessageBox.Show("The file selected was not a valid XBF file", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
#if !DEBUG
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Generic ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
#endif

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
