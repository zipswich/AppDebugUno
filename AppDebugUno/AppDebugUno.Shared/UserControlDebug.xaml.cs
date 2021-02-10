using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppDebugUno.Shared
{
    public sealed partial class UserControlDebug : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        bool _bShow = false;
        public bool bShow
        {
            get { return _bShow; }
            set
            {
                if (_bShow != value)
                {
                    _bShow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public UserControlDebug()
        {
            this.InitializeComponent();
        }

        private async void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Tapped");
        }
        public async void Show()
        {
            try
            {
                bShow = true;
                await Task.Delay(4000);
                bShow = false;
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
