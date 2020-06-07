using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CTUWP
{        

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardGallery : Page
    {
        public ObservableCollection<CardItem> itemcollection = new ObservableCollection<CardItem>();
        public CardGallery()
        {
            this.InitializeComponent();
            //setImageUri(img);
            App.showBackButton();
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                Frame.Navigate(typeof(selectPerson));
            };
        }

        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string personId = ((Parameters)e.Parameter).selectedId;
            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/cards/c/" + personId);
            ApiCommunicator catUri = new ApiCommunicator(apiConfig);
            JsonArray cardsReturned = await catUri.getArray();

            
            foreach(var cardString in cardsReturned)
            {
                JsonObject card = cardString.GetObject().GetObject();
                addImage(card.GetNamedString("image"), card.GetNamedString("description"));
            }
        }


        private async void addImage(string url, string cardName)
        {
            var rass = RandomAccessStreamReference.CreateFromUri(new Uri(url));
            using (IRandomAccessStream stream = await rass.OpenReadAsync())
            {
                try
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    itemcollection.Add(new CardItem { ImageItem = bitmapImage, name = cardName });
                }
                catch
                {

                }
            }
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (CardItem)e.ClickedItem;

            if (AdaptiveStates.CurrentState == NarrowState)
            {
                Frame.Navigate(typeof(DetailPage), clickedItem);
            }
        }

    }
}
