using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace BackEndNalogaWebService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] PoisciDatoteke(string imeDatoteke)
        {
            //Pot do json
            string fileName = "E:\\tree_structure.json";

            string jsonString = File.ReadAllText(fileName);

            var rootLists = JsonConvert.DeserializeObject<List<Content>>(jsonString);

            var poti = new List<string>();
            var rez = new string[0];

            rez = Rekurzija(rootLists, poti, rez, imeDatoteke);

            if (rez.Length == 0)
            {
                Array.Resize(ref rez, rez.Length + 1);
                rez[rez.Length - 1] = "Datoteka z tem imenom ne obstaja";
                return rez;
            }
            else
            {
                return rez;
            }

        }
        [WebMethod]
        static string[] Rekurzija(List<Content> contentLists, List<string> poti, string[] rez, string imeDatoteke)
        {

            foreach (var content in contentLists)
            {
                if (content.contents != null && content.type == "directory")
                {
                    if (content.name.Contains("/"))
                    {
                        poti.Add(content.name);
                    }
                    else
                    {
                        poti.Add("/" + content.name);
                    }
                    rez = Rekurzija(content.contents, poti, rez, imeDatoteke);
                    poti.RemoveAt(poti.Count - 1);

                }
                else if (content.type == "file" && content.name == imeDatoteke)
                {
                    poti.Add("/" + content.name);
                    string test = "";
                    foreach (var potke in poti)
                    {
                        test += potke;
                    }
                    Array.Resize(ref rez, rez.Length + 1);
                    rez[rez.Length - 1] = test;
                    poti.RemoveAt(poti.Count - 1);
                }
            }
            return rez;
        }
    }
    public class Content
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<Content> contents { get; set; }
        public string target { get; set; }
    }
}
