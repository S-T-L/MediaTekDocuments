using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe Service
    /// </summary>
    public class Service
    {
        /// <summary>
        /// id du service
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// libelle du service
        /// </summary>
        public string Libelle { get; set; }

       /// <summary>
       /// valorise les propriétés
       /// </summary>
       /// <param name="id">id </param>
       /// <param name="libelle">libelle du service</param>
        public Service(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;

           

        }

       
    }
}

