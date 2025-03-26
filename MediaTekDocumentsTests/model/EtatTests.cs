using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class EtatTests
    {

        private const string id = "00005";
        private const string libelle = "détérioré";

        private static readonly Etat etat = new Etat(id, libelle);

        /// <summary>
        /// Teste le constructeur de la classe Etat
        /// </summary>
        [TestMethod()]
        public void EtatTest()
        {
            Assert.AreEqual(id, etat.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, etat.Libelle, "devrait réussir : libellé valorisé");
        }
    }
}