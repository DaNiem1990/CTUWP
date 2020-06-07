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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
            App.hideBackButton();
        }              

        private async void Button_Click(object sender, RoutedEventArgs args)
        {
            prepareApiRequestData(sender);

            ApiAddress apiConfig = prepareApiRequestConfig();
            Message.show("Trwa rejestrowanie proszę czekać", eBox);

            
            

            await HttpUserPost(apiConfig, eBox);
        }

        private void prepareApiRequestData(object dataContainer)
        {
            UserData.setAllFields(loginField.Text, nameField.Text, passwordField.Password, cPasswordField.Password, "");
        }

        private ApiAddress prepareApiRequestConfig()
        {
            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/users/register");
            
            return apiConfig;
        }

        private bool isPasswordSame(string password, string cPassword)
        {
            try
            {
                if (password.Equals(cPassword))
                {
                    return true;
                }
            }
            catch
            {
                
            }
            throw new Exception("Podane hasła nie są takie same");
            
        }

        private bool checkPassword(string password, string cPassword)
        {
            try
            {
                if(password.Length > 7)
                {
                    return true;
                }
            }
            catch
            {

            }
            throw new Exception("Hasło musi składać się conajmniej z siedmiu znaków");
        }

        public async Task HttpUserPost(ApiAddress apiConfig, TextBlock errorBox)
        {
            try
            {
                bool isPasswordsCorrect = isPasswordSame(UserData.getField("password"), UserData.getField("c_password"));
                bool isPasswordsLong = checkPassword(UserData.getField("password"), UserData.getField("c_password"));
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
