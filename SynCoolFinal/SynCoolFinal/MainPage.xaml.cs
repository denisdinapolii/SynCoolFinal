using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SynCoolFinal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Registrazione());
        }

        private async void btnAccedi_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=login&mail={txtMail.Text}&pass={txtPass.Text}";


            string response = await client.GetStringAsync(url);
            if (response.Contains("true"))
            {
                await DisplayAlert("Information", "Loggato correttamente..", "Ok");
            }
            else
            {
                await DisplayAlert("Alert", "Errore nella login", "Ok");
            }
        }
    }
}
