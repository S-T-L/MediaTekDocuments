using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Service
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="idservice"></param>
        /// <param name="nom"></param>
        public Service(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;

           

        }

       
    }
}

