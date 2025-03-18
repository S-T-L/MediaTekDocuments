using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;
using System.Linq;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }
        /// <summary>
        /// Getter sur les suivis
        /// </summary>
        /// <returns>Liste de suivis</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }
        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }

        public List<Abonnement> GetAbonnementsExpirants()
        {
            List<Abonnement> abonnementsExpirants = new List<Abonnement>();
            List<Revue> revues = GetAllRevues();

            foreach (Revue revue in revues)
            {
                List<Abonnement> abonnements = GetAbonnementsRevue(revue.Id);
                var expirants = abonnements.Where(o =>
                    o.DateFinAbonnement <= DateTime.Now.AddMonths(1) &&
                    o.DateFinAbonnement >= DateTime.Now).ToList();

                abonnementsExpirants.AddRange(expirants);
            }

            return abonnementsExpirants;
        }

    public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }
    

    /// <summary>
    /// récupère les exemplaires d'une revue
    /// </summary>
    /// <param name="idDocuement">id de la revue concernée</param>
    /// <returns>Liste d'objets Exemplaire</returns>
    public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            return access.GetAbonnementsRevue(idRevue);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        public bool CreerAbonnement (Abonnement abonnementRevue)
        {
            return access.CreerAbonnement(abonnementRevue);
        }
        

        /// <summary>
        /// récupère les commandes d'un document
        /// </summary>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            return access.GetCommandesDocument(idDocument);
        }

       
        /// <summary>
        /// Crée une nouvelle commande de document dans la bdd
        /// </summary>
        /// <param name="commandeDoc">commande de document concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDoc)
        {
            return access.CreerCommandeDocument(commandeDoc);
        }

        public bool ModiferCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.ModifierCommandeDocument(commandeDocument);
        }

        public bool SupprimerCommandeDocument(string idCommande)
        {
            return access.SupprimerCommandeDocument(idCommande);
        }
         public bool SupprimerAbonnementRevue (string idAbonnement)
        {

            return access.SupprimerCommandeRevue(idAbonnement);
        }

    }
}
