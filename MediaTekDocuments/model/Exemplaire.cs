using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// photo de l'exemplaire
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Date d'achat
        /// </summary>
        public DateTime DateAchat { get; set; }
        /// <summary>
        /// Id de l'état
        /// </summary>
        public string IdEtat { get; set; }
        /// <summary>
        /// id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="numero">numero</param>
        /// <param name="dateAchat">dateAchat</param>
        /// <param name="photo">photo</param>
        /// <param name="idEtat">idEtat</param>
        /// <param name="idDocument">idDocument</param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
        }

    }
}
