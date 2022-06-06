using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class wrapPage : FlyoutPage
    {
        public string mail { get; set; }
        public wrapPage()
        {
            InitializeComponent();
            this.mail = Preferences.Get("mail", "default_value");
            flyout.listview.ItemSelected += OnSelectedItemAsync;
        }

        private void OnSelectedItemAsync(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutItemPage;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
                flyout.listview.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}