using Plugin.FirebaseStorage;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {

        HttpClient client;
        public string mail { get; set; }
        public Profile()
        {
            InitializeComponent();
            this.mail=Preferences.Get("mail", "default_value");
            client = new HttpClient();
            viewUser();
        }

        private async void viewUser()
        {
            try
            {
                string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getUserByMail&mail={this.mail}";
                string xml = await client.GetStringAsync(url);
                message_user res = (message_user)util.xmlDeserialization(typeof(message_user), xml);

                txtCognome.Text = res.user.Cognome;
                txtCitta.Text = res.user.Citta;
                data.Date = res.user.DataNascita;
                txtDescrizione.Text = res.user.Descrizione;
                txtmail.Text = res.user.Email;
                txtUsername.Text = res.user.Username;

                if (res.user.IsTutor == 0)
                {
                    txtRuolo.Text = "Studente";
                }
                else
                {
                    txtRuolo.Text = "Tutor";
                }
                txtNome.Text = res.user.Nome;

                var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("img_profile").Child(res.user.Pic);
                var img = await reference.GetStreamAsync();
                imageProfile.Source = ImageSource.FromStream(() => img);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void btnChangeDati_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new listViewAppunti());
        }
    }
}