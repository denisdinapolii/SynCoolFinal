using System;
using Plugin.Media;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using System.Xml.Serialization;
using System.IO;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : ContentPage
    {
        HttpClient client;
        string name{ get; set; }
        public Registrazione()
        {
            InitializeComponent();
            client = new HttpClient();
            writeComboBox();
        }

        private async void writeComboBox()
        {
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getAllScuole";
            string xml=await client.GetStringAsync(url);

            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (StringReader reader = new StringReader(xml))
            {
               Root test = (Root)serializer.Deserialize(reader);
               cmbScuole.ItemsSource = test.Scuole.Scuola;
            }
        }

        private async void btnRegistrati_Clicked(object sender, EventArgs e)
        {
            int tutor;
            if (rdbTutor.IsChecked)
                tutor = 0;
            else
                tutor = 1;
            string url = "";
            string d=data.Date.ToString("yyyy-MM-dd");
            if (name is null)
                url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=post&action=register&username={txtUser.Text}&nome={txtNome.Text}&cognome={txtCognome.Text}&mail={txtMail.Text}&pass={txtPassword.Text}&dataN={d}&foto=NULL&desc={txtDesc.Text}&citta={txtCitta.Text}&tutor={tutor}&indirizzo={txtIndirizzo.Text}&scuola={(cmbScuole.SelectedItem as Scuola).ID}";
            else
                url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=post&action=register&username={txtUser.Text}&nome={txtNome.Text}&cognome={txtCognome.Text}&mail={txtMail.Text}&pass={txtPassword.Text}&dataN={d}&foto={name}&desc={txtDesc.Text}&citta={txtCitta.Text}&tutor={tutor}&indirizzo={txtIndirizzo.Text}&scuola=1";


            string response = await client.GetStringAsync(url);
            if (response.Contains("true"))
            {
                await DisplayAlert("Information", "Utente registrato correttamente.", "Ok");
            }
            else
            {
                await DisplayAlert("Alert", "Utente già registrato.", "Ok");
            }
        }

        private async void btnUploadImage_Clicked(object sender, EventArgs e)
        { //! added using Plugin.Media;
            try
            { 
                await CrossMedia.Current.Initialize();

                //// if you want to take a picture use this
                // if(!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                /// if you want to select from the gallery use this
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                    return;
                }

                //! added using Plugin.Media.Abstractions;
                // if you want to take a picture use StoreCameraMediaOptions instead of PickMediaOptions
                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                // if you want to take a picture use TakePhotoAsync instead of PickPhotoAsync
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);


                if (imageProfilo == null)
                {
                    await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
                    return;
                }

                imageProfilo.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                name= selectedImageFile.Path.Split('/')[selectedImageFile.Path.Split('/').Length-1];                        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}