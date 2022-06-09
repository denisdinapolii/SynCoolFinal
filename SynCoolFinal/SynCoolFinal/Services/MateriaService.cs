using SynCoolFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SynCoolFinal.Services
{
    public class MateriaService
    {
        public static async Task<List<Appunti>> MateriaToNameAsync(List<Appunti> l) 
        {
            for (int i = 0; i < l.Count; i++)
            {
                string c = await getNameMateriaById(l.ElementAt(i).Materia);
                l.ElementAt(i).Materia = c;
            }
            return l;
        }

        private static async Task<string> getNameMateriaById(string id)
        {
            string url = $"http://barclayspremierleague.altervista.org/webService/index.php?method=get&action=getNameMateriaByID&id={id}";
            HttpClient client = new HttpClient();
            string resp = await client.GetStringAsync(url);
            message_base mex = (message_base)util.xmlDeserialization(typeof(message_base), resp);
            return mex.Messaggio;
        }
    }
}
