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
            MessagingCenter.Subscribe<Object, string>(this, "mail", (s, res) =>
            {
                this.mail = Convert.ToString(res);
            });
            client = new HttpClient();
            writeCmbMateria();
        }

        private async void writeCmbMateria()
        {
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getMaterie";
            string xml = await client.GetStringAsync(url);
            try
            {
                message_materie res = (message_materie)util.xmlDeserialization(typeof(message_materie), xml);
                cmbMateria.ItemsSource = res.materie.Materia;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            
            var file = await FilePicker.PickAsync();
            string filename = "";
            var file_upload = await file.OpenReadAsync();

            if (txtFilename.Text != null)
            {
                filename = txtFilename.Text;
            }
            else 
            {
                filename = file.FileName;
            }


            var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").Child(filename);
            var metadata = new MetadataChange
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    ["id_utente"] = this.mail,
                    ["id_categoria"]=(cmbMateria.SelectedItem as message_materie.Materia).ID.ToString()
                }
            };

            await reference.PutStreamAsync(file_upload, metadata);
        }
    }
}