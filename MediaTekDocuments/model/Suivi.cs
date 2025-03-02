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
        public int Id { get; }
        public string Libelle { get; }

        public Suivi(int id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
