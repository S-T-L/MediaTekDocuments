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
    public class UtilisateurTests
    {
        private const string login = "NPrenom";
        private const string nom = "testNom";
        private const string prenom = "testPrénom";
        private const string password = "mdp";
        private const string idService = "2";
        private const string libelle = "culture";
        private static readonly Utilisateur utilisateur = new Utilisateur(nom, prenom, idService,password, login, libelle );
       
        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(login, utilisateur.Login, "devrait réussir : login valorisé");
            Assert.AreEqual(nom, utilisateur.Nom, "devrait réussir :nom valorisé");
            Assert.AreEqual(prenom, utilisateur.Prenom, "devrait réussir : prenom valorisé");
            Assert.AreEqual(password, utilisateur.Password, "devrait réussir : pwd valorisé");
            Assert.AreEqual(libelle, utilisateur.Service.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}