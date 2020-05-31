using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CTUWP
{
    public class Message
    {
        public static void show(string errorMessage, TextBlock errorBox)
        {
            errorBox.Text = errorMessage;
        }
    }
}
