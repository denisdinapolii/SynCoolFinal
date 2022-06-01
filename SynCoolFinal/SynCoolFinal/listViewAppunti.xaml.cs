using Plugin.FirebaseStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SynCoolFinal.ViewModels;
using Xamarin.Essentials;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class listViewAppunti : ContentPage
    {
        public string mail { get; set; }
        public List<AppuntiViewModel> l {get; set;}
        public listViewAppunti()
        {
            InitializeComponent();
            this.l= new List<AppuntiViewModel>();
            this.mail = Preferences.Get("mail", "default_value");
            viewAppunti();
        }

        private async void viewAppunti()
        {
            // All
            var result = await CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").ListAllAsync();
            var items = result.Items.ToList();
            await Task.Run(() => Task.Run(() => createView(items)));
           
        }

        private async Task createView(List<IStorageReference> l) 
        {
            AppuntiViewModel c;
            for (int i = 0; i < l.Count; i++)
            {
                c = new AppuntiViewModel();
                var meta= await l.ElementAt(i).GetMetadataAsync();
                string mailT = meta.CustomMetadata["id_utente"];
                if (mailT==this.mail) 
                {
                    c.Nome = meta.Name;
                    c.Materia = meta.CustomMetadata["id_categoria"];
                    this.l.Add(c);
                }
            }
            Dispatcher.BeginInvokeOnMainThread(() => {  listAppunti.ItemsSource= this.l; });
        }

    }
}