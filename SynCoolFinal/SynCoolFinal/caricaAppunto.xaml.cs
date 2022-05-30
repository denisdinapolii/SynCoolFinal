using Plugin.FirebaseStorage;
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
    public partial class caricaAppunto : ContentPage
    {
        HttpClient client;
        public string mail { get; set; }
        public caricaAppunto()
        {
            InitializeComponent();
            this.mail = mail;
            client = new HttpClient();
            writeCmbMateria();
        }

        private async void writeCmbMateria()
        {
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getMaterie";
            string xml = await client.GetStringAsync(url);
            message_base res = (message_base)util.xmlDeserialization(typeof(message_base), xml);
            //cmbMateria.ItemsSource = res.Messaggio;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync();
            string filename = file.FileName;
            var file_upload = await file.OpenReadAsync();

            var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").Child(filename);
            var metadata = new MetadataChange
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    ["id_utente"] = this.mail,
                    ["id_categoria"]=cmbMateria.SelectedItem as string
                }
            };

            await reference.PutStreamAsync(file_upload, metadata);
        }
    }
}