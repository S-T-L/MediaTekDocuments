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
        /// <summary>
        /// Récupère la liste des abonnements expirants
        /// </summary>
        /// <returns>liste d'objets abonnement dont la date de fin abonnement est inférieure à 30 jours</returns>
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

        /// <summary>
        /// Réxupère toutes les commandes
        /// </summary>
        /// <returns>Liste d'objets commande</returns>
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
        /// <summary>
        /// récupère les abonnements associés à une revue
        /// </summary>
        /// <param name="idRevue">id de la revue</param>
        /// <returns>liste d'objets abonnement</returns>
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
        /// <summary>
        /// Crée un abonnement pour une revue 
        /// </summary>
        /// <param name="abonnementRevue">objet revue concerné</param>
        /// <returns>true si création </returns>
        public bool CreerAbonnement(Abonnement abonnementRevue)
        {
            return access.CreerAbonnement(abonnementRevue);
        }


        /// <summary>
        /// Retourne la liste des commandes d'un document livre, dvd
        /// </summary>
        /// <param name="idDocument">id du document</param>
        /// <returns>Liste d'objets commadeDocument</returns>
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
        /// <summary>
        /// Modifie une commande de livre ou dvd 
        /// </summary>
        /// <param name="commandeDocument">objet commandeDocument</param>
        /// <returns>true si la modification a bien été effectuée</returns>
        public bool ModiferCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.ModifierCommandeDocument(commandeDocument);
        }
        /// <summary>
        /// Supprime une commande en bdd
        /// </summary>
        /// <param name="idCommande">id de la commande</param>
        /// <returns>true si suppression</returns>
        public bool SupprimerCommandeDocument(string idCommande)
        {
            return access.SupprimerCommandeDocument(idCommande);
        }
        /// <summary>
        /// Supprime une commande à un abonnement de revue
        /// </summary>
        /// <param name="idAbonnement">id abonnement</param>
        /// <returns>true si suppression</returns>
        public bool SupprimerAbonnementRevue(string idAbonnement)
        {

            return access.SupprimerCommandeRevue(idAbonnement);
        }

    }
}
