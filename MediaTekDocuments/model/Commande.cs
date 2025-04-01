using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    
    /// <summary>
    /// Classe Commande avec informations communes pour toutes les commandes 
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// id commande
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// date de la commande
        /// </summary>
        public DateTime DateCommande { get; set; }
        /// <summary>
        /// montant de la commnande
        /// </summary>
        public double Montant { get; set; }
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="montant">montant de la commande</param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
  