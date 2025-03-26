using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe Utilisateur
    /// </summary>
    public class Utilisateur
    {
      
        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Nom { get; }
        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        public string Prenom { get; }
        /// <summary>
        /// Id du service de l'utilisateur
        /// </summary>
        public string IdService { get; set; }
        /// <summary>
        /// mot de passe de l'utilisateur
        /// </summary>
        public string Password { get; }
        /// <summary>
        /// login utilisateur
        /// </summary>
        public string Login { get; }
        /// <summary>
        /// Service de l'utilisateur
        /// </summary>
        public Service Service { get; }
        /// <summary>
        /// Valorise les propriétéss
        /// </summary>
        /// <param name="nom">nom</param>
        /// <param name="prenom">prenom</param>
        /// <param name="idService">idService</param>
        /// <param name="password">password</param>
        /// <param name="login">login</param>
        /// <param name="libelle">libelle</param>
        public Utilisateur(string nom, string prenom, string idService, string password, string login, string libelle)
        {
            
            this.Nom = nom;
            this.Prenom = prenom;
            this.Service = new Service(idService, libelle);
            this.Password = password;
            this.Login = login;
            

            

        }
    }
}

