using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CTUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyProfile : Page
    {
        public MyProfile()
        {
            
            this.InitializeComponent();
            getUserData();

            App.hideBackButton();
        }


        public async void getUserData()
        {
            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/users/details");
            ApiCommunicator apiCommunicator = new ApiCommunicator(apiConfig);
            JsonObject resp = await apiCommunicator.get();
            UserData.setUserDetails(resp);
            Message.show(UserData.getBasicData() +"\n" + "Id: " + UserData.getId(), errorBox);

        }

        private void Mk_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(selectPerson));
        }

        private void Do_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Offerts));
        }
    }
}
