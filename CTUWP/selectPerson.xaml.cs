using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class selectPerson : Page
    {
        public selectPerson()
        {
            this.InitializeComponent();
            App.showBackButton();
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                Frame.Navigate(typeof(MyProfile));
            };
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/categories");
            ApiCommunicator catUri = new ApiCommunicator(apiConfig);
            JsonArray personsReturned = await catUri.getArray();
            //int mt = 00;
            foreach(JsonValue person in personsReturned)
            {
                //JsonObject personObject = JsonObject.Parse(person.ToString());
                string id = person.GetObject().GetNamedNumber("id").ToString();
                Button button = new Button();
                button.Click += SelectPersonButton_Click;
                button.Tag = id;
                button.Margin = new Thickness(0,10,0,0);
                //button.Height = 50;
                button.Width = 200;
                button.FontSize = 18;
                button.Content = person.GetObject().GetNamedString("name");

                //mt += 100;
                buttonsPlace.Children.Add(button);
                
            }

        }

        private void SelectPersonButton_Click(object sender, RoutedEventArgs e)
        {
            
            string personId = ((Button)sender).Tag.ToString();
            Parameters parameters = new Parameters { selectedId = personId };
            Frame.Navigate(typeof(CardGallery), parameters);
        }

        //private void Button_Click()
       // {
            
            
       // }
    }
}
