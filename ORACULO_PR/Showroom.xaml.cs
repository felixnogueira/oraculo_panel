using Microsoft.Web.WebView2.Core;
using System.Windows;
using System.Windows.Controls;


namespace ORACULO_PR
{
    /// <summary>
    /// Interação lógica para Showroom.xam
    /// </summary>
    public partial class Showroom : UserControl
    {
        private const string defaultURL = "https://vila360.com.br/tour/mitsubishioffline/index.html?installed=true";

        public Showroom()
        {
            InitializeComponent();
            Loaded += Showroom_Loaded;
        }

        private void Showroom_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            var webView = new Microsoft.Web.WebView2.Wpf.WebView2();
            var env = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView.EnsureCoreWebView2Async(env);
            browserGrid.Children.Add(webView);
        }

        private CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions()
        {
            AdditionalBrowserArguments = "--profile-directory=Default --app-id=ahjilncfcdjileccgmjabkopmdppapkb --app-url=https://vila360.com.br/tour/mitsubishioffline/index.html?installed=true --app-launch-source=4"
        };

    }


}

