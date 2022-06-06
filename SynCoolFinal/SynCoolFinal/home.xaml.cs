using Firebase.Storage;
using Plugin.FirebaseStorage;
using Plugin.Media;
using SynCoolFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class home : ContentPage
    {
        public home()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Ok", "Add", "Ok");
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var listTemp=await CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").ListAsync(10);
            var items = listTemp.Items.ToList();
            items = items.Where(s => s.Name.Contains(e.NewTextValue)).ToList();
            await Task.Run(() => createView(items)); 
        }

        private async Task createView(List<IStorageReference> l)
        {
            List<AppuntiViewModel> list_temp = new List<AppuntiViewModel>();
            AppuntiViewModel c;
            for (int i = 0; i < l.Count; i++)
            {
                c = new AppuntiViewModel();
                c.Nome = l.ElementAt(i).Name;
                c.Materia = (await l.ElementAt(i).GetMetadataAsync()).CustomMetadata["id_categoria"];
                list_temp.Add(c);
            }
        
            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = list_temp; });
        }

        private void listAppunti_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}