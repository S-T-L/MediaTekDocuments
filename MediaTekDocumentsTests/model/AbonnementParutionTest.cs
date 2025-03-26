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
    public class AbonnementParutionTest
    {


        readonly Abonnement abonnement = new Abonnement("1", new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Local), 10, new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Local), null);
        readonly DateTime dateCommande = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Local);
        readonly DateTime dateFinAbonnement = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Local);

        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            // Cas 1: La date de parution est exactement la date de commande
            Assert.IsFalse(abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Local)), "La date de parution ne doit pas être égale à la date de commande");

            // Cas 2: La date de parution est exactement la date de fin d'abonnement
            Assert.IsFalse(abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Local)), "La date de parution ne doit pas être égale à la date de fin d'abonnement");

            // Cas 3: La date de parution est entre la date de commande et la date de fin d'abonnement
            Assert.IsTrue(abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Local)), "La date de parution devrait être valide, car elle est entre les deux dates");

            // Cas 4: La date de parution est avant la date de commande
            Assert.IsFalse(abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, new DateTime(2025, 2, 28, 0, 0, 0, DateTimeKind.Local)), "La date de parution devrait être invalide, car elle est avant la date de commande");

            // Cas 5: La date de parution est après la date de fin d'abonnement
            Assert.IsFalse(abonnement.ParutionDansAbonnement(dateCommande, dateFinAbonnement, new DateTime(2025, 5, 2, 0, 0, 0, DateTimeKind.Local)), "La date de parution devrait être invalide, car elle est après la date de fin d'abonnement");
        }


    }
}