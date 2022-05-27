using Firebase.Storage;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SynCoolFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class home : ContentPage
    {
        public home()
        {
            InitializeComponent();
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            FirebaseStorage storage= new FirebaseStorage("syncool-6f25c.appspot.com");
            var file = await FilePicker.PickAsync();
            string filename = file.FileName;
            string ext = file.ContentType;
            string fullpath=file.FullPath;
            var stream = await file.OpenReadAsync();

            FirebaseStorageReference reference = storage.Child("appunti").Child(filename);

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = reference.PutAsync(stream);

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;                                                //https://firebasestorage.googleapis.com/v0/b/syncool-6f25c.appspot.com/o/appunti%2FAppunti.docx?alt=media&token=6263c65e-5162-4508-a2e6-bc28be8410d0
        
        }



    }
}