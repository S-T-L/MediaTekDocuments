using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{

    /// <summary>
    /// Classe CommandeDocument : Hérite de la classe Document 
    /// </summary>
    public class CommandeDocument : Commande
    {
        public int NbExemplaire { get; }
        public string IdLivreDvd { get; }
        public int IdSuivi { get; }
        public string LibelleSuivi { get; }

        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd, int idSuivi, string libelle) 
            : base (id, dateCommande, montant)
        {
            this.NbExemplaire = nbExemplaire;
            this.IdLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
            this.LibelleSuivi = libelle;
        }
    }
}
