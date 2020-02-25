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

        string _sDebug = "This text should change after each button click because x:DefaultBindMode=\"OneWay\".";
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

        string _sDummy = "Dummy string needed for calling a converter";
        public string sDummy
        {
            get { return _sDummy; }
            set
            {
                if (_sDummy != value)
                {
                    _sDummy = value;
                    NotifyPropertyChanged();
                }
            }
        }

        bool _bBool = true;
        public bool bBool
        {
            get { return _bBool; }
            set
            {
                if (_bBool != value)
                {
                    _bBool = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private void border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            sDebug = "Pointer released at: " + DateTime.Now;
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            bBool = !bBool;
            sDebug = DateTime.Now.ToString();
        }
    }

    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }


    public sealed class TextConverterDebug : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "Converter is called for ConverterParameter:" +(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return "Converted Back";
        }
    }

}
