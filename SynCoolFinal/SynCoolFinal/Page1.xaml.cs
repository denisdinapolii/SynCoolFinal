using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

            string response = await client.GetStringAsync(url);

            document = XDocument.Parse(response);
            document.Descendants("Success");

            if( Boolean.Parse(document.Element("Success").Value))
                await DisplayAlert("Information", document.Element("Messaggio").Value, "Ok");
            else
                await DisplayAlert("Attention", document.Element("Messaggio").Value, "Ok");
        }
    }
}