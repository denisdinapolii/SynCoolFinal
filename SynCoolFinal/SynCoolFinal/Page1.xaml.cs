using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        XDocument document;
        public Page1()
        {
            InitializeComponent();
            document= new XDocument();
        }


        private async void btnRecoveryPassword_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
           
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=resPass&mailOus={txtMailOus.Text}";

            string xml = await client.GetStringAsync(url);

            XmlSerializer serializer = new XmlSerializer(typeof(recovery));
            using (StringReader reader = new StringReader(xml))
            {
                recovery test = (recovery)serializer.Deserialize(reader);
                await DisplayAlert("Information", test.Messaggio as string, "Ok");
            }
        }
    }
}