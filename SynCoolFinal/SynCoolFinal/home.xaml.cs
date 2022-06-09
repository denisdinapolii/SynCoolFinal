using Firebase.Storage;
using Plugin.FirebaseStorage;
using Plugin.Media;
using SynCoolFinal.Models;
using SynCoolFinal.Services;
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
        public home()
        {
            InitializeComponent();
            writeCmbMateria();
        }

        
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Appunti> l;
            l =  await AppuntiService.ToList(await AppuntiService.getList());
            l = AppuntiService.listBySearchBar(e.NewTextValue.ToLower(), l);
            l = await MateriaService.MateriaToNameAsync(l);
            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = l; });
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
            List<Appunti> l;
            if (cmbMaterie.SelectedIndex != 0) //se una materia è selezionata
            {
                var m = cmbMaterie.SelectedItem as message_materie.Materia;
                l = await AppuntiService.listByItem(m.ID);
            }
            else
                l = await AppuntiService.ToList(await AppuntiService.getList());


            l = await MateriaService.MateriaToNameAsync(l);
            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = l; });
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