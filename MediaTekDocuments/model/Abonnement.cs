using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// model
    /// </summary>
    internal class NamespaceDoc
    {

    }
    /// <summary>
    /// Classe Abonnement : hérite de la classe Commande
    /// </summary>
    public class Abonnement  : Commande
    {
       /// <summary>
       /// date de fin d'abonnement
       /// </summary>
        public DateTime DateFinAbonnement { get; set; }

        /// <summary>
        /// id revue
        /// </summary>
        public string IdRevue { get; set; }

               
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="montant">montant de la commande</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <param name="idRevue">id de la revue</param>
        public Abonnement(string id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue) : base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
          
        }

        /// <summary>
        /// Retourne vrai si la date de parution est entre dateCommande et dateFinAbonnement
        /// </summary>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <param name="dateParution">date de parution</param>
        /// <returns>true si date parution entre date commande et date fin</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (DateTime.Compare(dateCommande, dateParution) < 0 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }



    }
}
