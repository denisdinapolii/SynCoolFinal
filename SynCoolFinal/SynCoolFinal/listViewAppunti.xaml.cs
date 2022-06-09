using Plugin.FirebaseStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SynCoolFinal.Models;
using Xamarin.Essentials;
using System.IO;
using System.Net.Http;
using System;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using SynCoolFinal.Services;

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
            Init();
        }

        private async void Init()
        {
            var l=  await AppuntiService.listByUser(this.mail);
            l = await MateriaService.MateriaToNameAsync(l);
            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = l; } );
        }

       
        private async void listAppunti_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var appunto = e.Item as Appunti;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti").Child(appunto.Nome);
                var stream =await reference.GetStreamAsync();
                var url = await reference.GetDownloadUrlAsync();

                using (var memory = new MemoryStream()) 
                {
                    await stream.CopyToAsync(memory);
                    await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView("myfile.pdf","application/pdf", memory, PDFOpenContext.InApp);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Appunti> l = await AppuntiService.listBySearchBarAndUser(e.NewTextValue.ToLower(), this.mail);
            l = await MateriaService.MateriaToNameAsync(l);
            Dispatcher.BeginInvokeOnMainThread(() => { listAppunti.ItemsSource = l; });
        }


    }
}