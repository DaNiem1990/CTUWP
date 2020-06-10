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
        private string personId;
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
            personId = ((Parameters)e.Parameter).selectedId;

            ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/cards/c/" + personId);
            ApiCommunicator catUri = new ApiCommunicator(apiConfig);
            JsonArray cardsReturned = await catUri.getArray();

            apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/subcategories/");
            catUri = new ApiCommunicator(apiConfig);
            JsonArray catsReturned = await catUri.getArray();

            apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/users/details");
            catUri = new ApiCommunicator(apiConfig);
            JsonObject accountInfo = await catUri.get();


            //foreach(card in ) { 
            //accountInfo.GetObject().GetNamedObject("cards").GetObject();}

            Dictionary<string, double> cardsInMyPocket = new Dictionary<string, double>();

            foreach(string key in accountInfo.GetObject().GetNamedObject("cards").Keys)
            {
                cardsInMyPocket.Add(key, accountInfo.GetObject().GetNamedObject("cards").GetNamedObject(key).GetNamedNumber("qty"));
            }
                        
            int pos = 0;
            foreach (var catString in catsReturned) {
                JsonObject cat = catString.GetObject();

                var cardsList = cardsReturned.Where(x => x.GetObject().GetNamedObject("subcategory").GetNamedNumber("id") == cat.GetNamedNumber("id"));
                addImageToCollection( cat.GetNamedString("name"));
                pos++;
                //addImageToCard("http://ct.zobacztu.pl/images/arrow_right.jpg", )

                foreach (var cardString in cardsList)
                {
                    JsonObject card = cardString.GetObject();
                    addImageToCollection(card.GetNamedString("description"));
                    addImageToCard(card.GetNamedString("image"), pos);
                    string cardId = card.GetNamedValue("id").ToString();
                    int qty = (
                        cardsInMyPocket.ContainsKey(cardId)
                        ? (int)cardsInMyPocket[cardId]
                        : 0);
                    setData(pos++, 
                        cardId,
                        card.GetNamedObject("category").GetNamedString("name"),
                        card.GetNamedObject("subcategory").GetNamedString("name"), 
                        qty);
                }
            }
        }

        private void setData(int index, string cardId, string cat, string subCat, int qty)
        {
            itemcollection[index].id = cardId;
            itemcollection[index].categoryName = cat;
            itemcollection[index].subCategory = subCat;
            itemcollection[index].quantity = qty;

        }

        private void addImageToCollection(string imageName)
        {
            var bitmapImage = new BitmapImage();
            itemcollection.Add(new CardItem { ImageItem = bitmapImage, name = imageName });
        }

        private async void addImageToCard(string url, int index)
        {
            var rass = RandomAccessStreamReference.CreateFromUri(new Uri(url));
            using (IRandomAccessStream stream = await rass.OpenReadAsync())
            {
                try
                {
                    itemcollection[index].ImageItem.SetSource(stream);
                }
                catch
                {

                }
            }
        }
        
        private async void addCardToUser(string selectedId)
        {
            try
            {
                ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/users/cards/add/" + selectedId);
                ApiCommunicator catUri = new ApiCommunicator(apiConfig);
                JsonObject accountInfo = await catUri.put();
                itemcollection.Where(x => x.id == selectedId).First().quantity++;
                
            }
            catch
            {

            }
        }

        private async void removeCardFromUser(string selectedId)
        {
            try
            {
                ApiAddress apiConfig = new ApiAddress("http://ct.zobacztu.pl", "/api/users/cards/sub/" + selectedId);
                ApiCommunicator catUri = new ApiCommunicator(apiConfig);
                JsonObject accountInfo = await catUri.put();
                if(itemcollection.Where(x => x.id == selectedId).First().quantity == 0)
                {

                }
                else
                {
                    itemcollection.Where(x => x.id == selectedId).First().quantity--;
                }

            }
            catch
            {

            }
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            string cardId = ((Button)sender).Tag.ToString();
            addCardToUser(cardId);
            Parameters parameters = new Parameters { selectedId = personId };
            Frame.Navigate(typeof(CardGallery), parameters);
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            string cardId = ((Button)sender).Tag.ToString();
            removeCardFromUser(cardId);
            Parameters parameters = new Parameters { selectedId = personId };
            Frame.Navigate(typeof(CardGallery), parameters);
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
