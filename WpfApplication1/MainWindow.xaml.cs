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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static URLCache urlCache;

        public MainWindow()
        {         
            InitializeComponent();
            // instanciate url cache
            urlCache = new URLCache();
            // load all created TinyUrls from the database
            urlCache.LoadURLs();
            DisplayResults();
        }

        private void DisplayResults()
        {
            string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string file = String.Format("{0}\\urlcache.txt", myDocuments);

            if (File.Exists(file))
            { 
                StreamReader reader = new StreamReader(file);

                TextBox tb = this.Results;
                tb.Clear();
                tb.Margin = new Thickness(10);
                tb.TextWrapping = TextWrapping.Wrap;
                string displayResult = reader.ReadToEnd();
                tb.AppendText(displayResult);  // display the result in the text box

                reader.Close();
            }
        }
        // Hide browser button
        private void HideMainBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserPopup.IsOpen = false;
        }
        // click on the Navigate button
        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserPopup.IsOpen = true;
            // get the long URL to navigate from the cache
            string urlToNavigate = urlCache.GetUrlCache()[NavigateToUrl.Text.Trim()];
            MainBrowser.Navigate(urlToNavigate);
        }
        // click on Create TinyUrl button
        private void CreateTinyUrl_Click(object sender, RoutedEventArgs e)
        {
            string tinyUrl = urlCache.AddURL(URL.Text);

            if (tinyUrl == "")
            {
                MessageBox.Show("Incorrect or empty URL is entered");
            }
            else if (tinyUrl == "TinyUrl is already created")
            {
                MessageBox.Show("TinyUrl is already created");
            }
            else
            {
                DisplayResults();
            }
        }
    }
}
