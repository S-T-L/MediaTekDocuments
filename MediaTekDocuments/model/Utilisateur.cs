using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
      

        public string Nom { get; }

        public string Prenom { get; }

        public string IdService { get; set; }

        public string Password { get; }

        public string Login { get; }
        
        public Service Service { get; }

        public Utilisateur(string nom, string prenom, string idService, string password, string login, string libelle)
        {
            
            this.Nom = nom;
            this.Prenom = prenom;
            this.Service = new Service(idService, libelle);
            this.Password = password;
            this.Login = login;
            

            Console.WriteLine($"Utilisateur: {this.Login}, Service: {this.Service.Libelle}");

        }
    }
}

