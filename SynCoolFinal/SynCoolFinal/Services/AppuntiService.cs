using Plugin.FirebaseStorage;
using SynCoolFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynCoolFinal.Services
{
    public class AppuntiService
    {
        public static IStorageReference reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("appunti");

        //ritorna la list di tipo List<IStorageReference>
        public static async Task<List<IStorageReference>> getList() 
        {
            return (await reference.ListAllAsync()).Items.ToList();
        }

        //ritorna la lista di appunti da far visionare a schermo
        public async static Task<List<Appunti>> ToList(List<IStorageReference> l)
        {
            List<Appunti> list_temp = new List<Appunti>();
            Appunti c;
            for (int i = 0; i < l.Count; i++)
            {
                c = new Appunti();
                c.Nome = l.ElementAt(i).Name;
                c.Materia = (await l.ElementAt(i).GetMetadataAsync()).CustomMetadata["id_categoria"];
                list_temp.Add(c);
            }
            return list_temp;
        }

        //il metodo secondo i parametri salva l'appunto nella cartella appunti (storage firebase)
        public static async Task saveAppunto(string filename, string mail, int ID, Stream stream)
        {
            var metadata = new MetadataChange
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    ["id_utente"] = mail,
                    ["id_categoria"] = ID.ToString()
                }
            };
            await reference.Child(filename).PutStreamAsync(stream, metadata);
        }


        //il metodo controlla se il file è già stato caricato secondo l'utente collegato
        public static async Task<bool> isSet(string mail, string filename)
        {
            try
            {
                var meta = await reference.Child(filename).GetMetadataAsync();
                if (meta.CustomMetadata["id_utente"] == mail)   //già presente il file
                    return true;
            }
            catch (Exception) { }
            return false;

        }

        //ritorna la lista di appunti(searchbar) caricati dall'utente da far visionare a schermo 
        public async static Task<List<Appunti>> listBySearchBarAndUser(string e, string user)
        {
            List<Appunti> l=await listByUser(user);
            l=listBySearchBar(e, l);
            return l;
        }

        //ritorna la lista di appunti caricati dall'utente da far visionare a schermo
        public async static Task<List<Appunti>> listByUser(string user)
        {
            List<Appunti> appunti = new List<Appunti>();
            var l = await getList();
            Appunti appunto;
            for (int i = 0; i < l.Count; i++)
            {
                var el = l.ElementAt(i);
                var meta=await el.GetMetadataAsync();
                if (meta.CustomMetadata["id_utente"] == user) 
                {
                    appunto = new Appunti();
                    appunto.Materia = meta.CustomMetadata["id_categoria"];
                    appunto.Nome = el.Name;
                    appunti.Add(appunto);
                }
            }
            return appunti;            
        }

        //ritorna la lista di appunti(selecteditem) da far visionare a schermo
        public async static Task<List<Appunti>> listByItem(int ID)
        {
            List<Appunti> list = await ToList(await getList());
            list = list.Where(x => x.Materia == ID.ToString()).ToList();
            return list;
        }

        //ritorna la lista di appunti(searchbar) da far visionare a schermo 
        public static List<Appunti> listBySearchBar(string e, List<Appunti> temp)
        {
            var list = temp.Where(s => s.Nome.ToLower().Contains(e)).ToList();     //lista che contiene il testo nella searchbar
            return list;
        }


        
    }
}
