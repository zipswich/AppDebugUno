using AppDebugUno.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppDebugUno
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string _sDebug = "Click the image";
        public string sDebug
        {
            get { return _sDebug; }
            set
            {
                if (_sDebug != value)
                {
                    _sDebug = value;
                    NotifyPropertyChanged();
                }
            }
        }

        uint uiWidth = 30;
        uint uiHeight = 30;

        public MainPage()
        {
            this.InitializeComponent();
            gridRoot.ColumnDefinitions[1].Width = new GridLength(0);
        }

        private void border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            sDebug = "Pointer released at: " + DateTime.Now;
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPageDebug));
        }

    }
}
