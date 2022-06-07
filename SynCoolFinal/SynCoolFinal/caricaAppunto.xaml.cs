using Plugin.FirebaseStorage;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Stream file_upload { get; set; }
        public string filename { get; set; }
        public string mail { get; set; }
        public int Materia { get; set; }
        public caricaAppunto()
        {
            InitializeComponent();
            this.mail=Preferences.Get("mail", "default_value");
            this.Materia = -1;
            this.filename = "";
            this.file_upload = null;
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
            string temp = "";// filename temp
            if (this.mail != null && this.mail != "")
            {
                if (this.Materia > -1 && this.filename != "" && this.file_upload != null)
                {
                    try
                    {
                        if (txtFilename.Text == "" || txtFilename.Text is null)
                            temp = this.filename;
                        else
                            temp = txtFilename.Text;


                        var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").Child(temp);
                        var metadata = new MetadataChange
                        {
                            CustomMetadata = new Dictionary<string, string>
                            {
                                ["id_utente"] = this.mail,
                                ["id_categoria"] = (cmbMateria.SelectedItem as message_materie.Materia).ID.ToString()
                            }
                        };

                        try
                        {
                            var meta = await reference.GetMetadataAsync();
                            if (meta.CustomMetadata["id_utente"] == this.mail)
                            {
                                await DisplayAlert("Attenzione", "Hai già caricato questo documento!", "Ok");
                                return;
                            }
                            await DisplayAlert("Attenzione", "Hai caricato con successo il tuo documento!", "Ok");
                            await reference.PutStreamAsync(file_upload, metadata);
                        }
                        catch (Exception)
                        {
                            await DisplayAlert("Attenzione", "Hai caricato con successo il tuo documento!", "Ok");
                            await reference.PutStreamAsync(file_upload, metadata);
                        }

                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Attenzione", "Completa i campi necessari!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Attenzione", "Scegli correttamente il file da caricare o scegli una materia", "Ok");
                }
            }
            else 
            {
                await DisplayAlert("Attenzione", "Per caricare un documento devi effettuare l'accesso", "Ok");
            }
        }

        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await FilePicker.PickAsync();
                this.file_upload = await file.OpenReadAsync();
                this.filename = file.FileName;
                if (txtFilename.Text is null || txtFilename.Text=="")
                {
                    txtFile.Text = this.filename;
                }
                else 
                {
                    txtFile.Text = txtFilename.Text;
                }

                txtExt.Text = file.ContentType;
            }
            catch (Exception)
            {
                await DisplayAlert("Attenzione", "Il file non può essere caricato in SynCool", "Ok");
            }
            
        }

        private void cmbMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMateria.Text = (cmbMateria.SelectedItem as message_materie.Materia).Nome;
        }

        private void txtFilename_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtFilename.Text == "") 
            {
                txtFile.Text=this.filename;
            }
            else 
            {
                txtFile.Text = txtFilename.Text;
            }
        }
    }
}