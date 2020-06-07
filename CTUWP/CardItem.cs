using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace CTUWP
{
    public class CardItem
    {
        public BitmapImage ImageItem { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string categoryName { get; set; }
        public string subCategory { get; set; }
        public int quantity { get; set; }
    }
}
