using LibXbf;
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

                StringBuilder sb = new StringBuilder();
                foreach(var field in xr.Header.GetType().GetFields())
                {
                    sb.AppendLine(string.Format("{0}: {1}", field.Name, field.GetValue(xr.Header)));
                }
                foreach (var property in xr.Header.GetType().GetProperties())
                {
                    sb.AppendLine(string.Format("{0}: {1}", property.Name, property.GetValue(xr.Header)));
                }

                sb.AppendLine();
                sb.AppendLine("Strings: ");
                foreach(var s in xr.StringTable.Values)
                {
                    sb.AppendLine(s);
                }

                sb.AppendLine();
                sb.AppendLine("Assemblies: ");
                foreach (var s in xr.AssemblyTable.Values)
                {
                    sb.AppendLine(string.Format("{0}: {1}", s.ProviderKind, xr.StringTable.Values[s.StringId]));
                }

                sb.AppendLine();
                sb.AppendLine("Type Namespaces: ");
                foreach (var s in xr.TypeNamespaceTable.Values)
                {
                    sb.AppendLine(string.Format("{0}: {1}", xr.StringTable.Values[xr.AssemblyTable.Values[s.AssemblyId].StringId], xr.StringTable.Values[s.StringId]));
                }

                sb.AppendLine();
                sb.AppendLine("Types: ");
                foreach (var s in xr.TypeTable.Values)
                {
                    sb.AppendLine(string.Format("{0}: {1} ({2})", xr.StringTable.Values[xr.TypeNamespaceTable.Values[s.NamespaceId].StringId], xr.StringTable.Values[s.StringId], s.Flags));
                }

                sb.AppendLine();
                sb.AppendLine("Properties: ");
                foreach (var s in xr.PropertyTable.Values)
                {
                    sb.AppendLine(string.Format("{0}: {1} ({2})", xr.StringTable.Values[xr.TypeTable.Values[s.TypeId].StringId], xr.StringTable.Values[s.StringId], s.Flags));
                }

                sb.AppendLine();
                sb.AppendLine("XML Namespaces: ");
                foreach (var s in xr.XmlNamespaceTable.Values)
                {
                    sb.AppendLine(string.Format("{0}", xr.StringTable.Values[s]));
                }

                txtOutput.Text = sb.ToString();
            }
        }
    }
}
