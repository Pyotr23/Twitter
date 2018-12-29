using HtmlAgilityPack;
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
using System.Windows.Shapes;

using TweetSharp;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private TwitterService twitterService;
        private OAuthRequestToken requestToken;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void OpenAuthorizePage()
        {
            const string consumerKey = "YGn1Aw1TcrTSXhP1RYrRt3ZnH";
            const string consumerSecret = "Icn6kzlL07LhXJXwuJqk10k4hPDiMZzoKGlREvLjRyo7WtFL42";

            twitterService = new TwitterService(consumerKey, consumerSecret);

            requestToken = twitterService.GetRequestToken();

            var uri = twitterService.GetAuthorizationUri(requestToken);

            browser.Navigate(uri);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenAuthorizePage();
        }

        private void browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            if (e.Uri.ToString() == "https://api.twitter.com/oauth/authorize")
            {
                var verifier = GetVerifierFromPage();
                var accessToken = twitterService.GetAccessToken(requestToken, verifier);
                twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                var mainWindow = new MainWindow { service = twitterService };                
                mainWindow.Show();
                this.Close();
            }
        }
        
        private string GetVerifierFromPage()
        {
            dynamic doc = this.browser.Document;
            var html = doc.documentElement.innerHtml;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var codeNode = htmlDoc.DocumentNode.SelectSingleNode("//code");
            return codeNode.InnerText;
        }

        
    }
}
