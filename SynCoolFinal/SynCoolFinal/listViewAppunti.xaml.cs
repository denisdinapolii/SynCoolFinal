using Plugin.FirebaseStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class listViewAppunti : ContentPage
    {
        public string mail { get; set; }
        public listViewAppunti()
        {
            InitializeComponent();
            this.mail = Preferences.Get("mail", "default_value");
        }

        private async void viewAppunti()
        {
            // All
            var result = await CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").ListAllAsync();
            var items = result.Items.ToList();
            List<BindingMyCell> l = new List<BindingMyCell>();
            BindingMyCell c = new BindingMyCell();
            
            for (int i = 0; i < items.Count; i++)
            {
                var reference = await items[i].GetMetadataAsync();
                if (reference.CustomMetadata["id_utente"] == this.mail) 
                {
                    c.name = reference.Name;
                    c.materia = reference.CustomMetadata["id_categoria"];
                    l.Add(c);
                }
                
            }

        }

    }
}