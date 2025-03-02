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
        public string Id { get; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }

        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
  