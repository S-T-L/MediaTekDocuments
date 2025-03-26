using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe Suivi : Récupération des étapes de suivi d'une commande
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// id du suivi
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// libelle du suivi
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="libelle">libelle</param>
        public Suivi(int id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// récupération du libellé
        /// </summary>
        /// <returns>libelle du suivi</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
