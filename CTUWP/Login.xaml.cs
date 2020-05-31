using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CTUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void Grid_login_enter(object sender, KeyRoutedEventArgs args)
        {
            bool isEnterPressed = args.Key == VirtualKey.Enter;
            if (isEnterPressed)
            {
                doLogin(sender, args);
            }
        }

        private async void doLogin(object sender, RoutedEventArgs args)
        {
            prepareApiRequestData(sender);

            ApiAddress apiConfig = prepareApiRequestConfig("Login");
            Message.show("Trwa logowanie proszę czekać", eBox);


            await HttpUserPost(apiConfig, eBox);
        }

        private async void Button_Click(object sender, RoutedEventArgs args)
        {
            doLogin(sender, args);
        }
        
        private void prepareApiRequestData(object dataContainer)
        {
            UserData.setEmailAndPassword(loginField.Text, passwordField.Password);
            
        }

        private ApiAddress prepareApiRequestConfig(string action)
        {
            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/login");
            
            return apiConfig;
        }

        public async Task HttpUserPost(ApiAddress apiConfig, TextBlock errorBox)
        {
            try
            {
                ApiCommunicator apiConnector = new ApiCommunicator(apiConfig);
                await apiConnector.post();
                goToMyProfile();
            }
            catch (Exception ex)
            {
                Message.show(ex.Message.ToString(), errorBox);
            }

        }
        private void goToMyProfile()
        {
            Frame.Navigate(typeof(MyProfile));
        }

    }
}
