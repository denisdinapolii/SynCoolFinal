using Firebase.Storage;
using Plugin.FirebaseStorage;
using Plugin.Media;
using SynCoolFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        List<Plugin.FirebaseStorage.IStorageReference> l=new List<IStorageReference> ();
        public home()
        {
            InitializeComponent();
            initItems();
            writeCmbMateria();
        }

        
        private async void initItems() 
        {
            var listTemp = await CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").ListAllAsync();
            var items = listTemp.Items.ToList();
            this.l = items;
            
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Plugin.FirebaseStorage.IStorageReference> temp = new List<IStorageReference>();
            temp = this.l.Where(s => s.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            await Task.Run(() => createView(temp)); 
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

        private async void writeCmbMateria()
        {
            HttpClient client = new HttpClient();
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getMaterie";
            string xml = await client.GetStringAsync(url);
            try
            {
                message_materie res = (message_materie)util.xmlDeserialization(typeof(message_materie), xml);
                res.materie.Materia.Insert(0, new message_materie.Materia(999, "Nessuna materia"));
                cmbMaterie.ItemsSource = res.materie.Materia;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async void cmbMaterie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaterie.SelectedIndex == 0) 
            {
                await Task.Run(()=> createView(this.l));
                return;
            }

            var m = cmbMaterie.SelectedItem as message_materie.Materia;

            await Task.Run(() => createViewByMateria(m));

        }


        private async Task createViewByMateria(message_materie.Materia m)
        {
            List<AppuntiViewModel> list_temp = new List<AppuntiViewModel>();
            AppuntiViewModel c;
            for (int i = 0; i < l.Count; i++)
            {
                c = new AppuntiViewModel();

                var meta = await this.l.ElementAt(i).GetMetadataAsync();
                if (int.Parse(meta.CustomMetadata["id_categoria"]) ==m.ID)
                {
                    c.Nome = l.ElementAt(i).Name;
                    c.Materia = meta.CustomMetadata["id_categoria"];
                    list_temp.Add(c);
                }
            }

            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = list_temp; });
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("OK", "Calendario", "Ok");
        }


        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            Preferences.Set("mail", "");
            DisplayAlert("Perfetto", "Sei stato disconnesso", "Ok");
        }
    }
}