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
        /// <summary>
        /// Nombre d'exemplaires
        /// </summary>
        public int NbExemplaire { get; }
        /// <summary>
        /// idLivredvd
        /// </summary>
        public string IdLivreDvd { get; }
        /// <summary>
        /// idSuivi
        /// </summary>
        public int IdSuivi { get; set; }
        /// <summary>
        /// libelle du suivi
        /// </summary>
        public string LibelleSuivi { get; }
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="montant">montant de la commande</param>
        /// <param name="nbExemplaire">nombre d'exemplaires</param>
        /// <param name="idLivreDvd">id livre dvd</param>
        /// <param name="idSuivi">idSuivi</param>
        /// <param name="libelle">libelle du suivi</param>
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
