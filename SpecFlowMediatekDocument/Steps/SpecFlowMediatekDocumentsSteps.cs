using System;
using TechTalk.SpecFlow;
using MediaTekDocuments.model;
using MediaTekDocuments.view;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpecFlowMediatekDocument.Steps
{
    [Binding]
    public class SpecFlowMediatekDocumentsSteps
    {
        private readonly FrmMediatek frmMediatek = new FrmMediatek(new Utilisateur("Azerty", "Marie", "1", "administratifMarie", "AMarie", "administratif"));

        [Given(@"Je saisis la valeur (.*) dans txbLivresNumRecherche")]
        public void GivenJeSaisisLaValeurDansTxbLivresNumRecherche(string valeur)
        {
            TextBox TxtValeur = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresNumRecherche"];
            frmMediatek.Visible = true;
            TxtValeur.Text = valeur;
        }
        
        [Given(@"Je sélectionne la valeur '(.*)' dans cbxLivresGenres")]
        public void GivenJeSelectionneLaValeurDansCbxLivresGenres(string liste)
        {
            ComboBox cbxLivresGenres = (ComboBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["cbxLivresGenres"];
            cbxLivresGenres.SelectedItem = liste;
        }
        
        [Given(@"Je sélectionne la valeur '(.*)' dans cbxLivresPublics")]
        public void GivenJeSelectionneLaValeurDansCbxLivresPublics(string liste)
        {
            ComboBox cbxLivresPublics = (ComboBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["cbxLivresPublics"];
            cbxLivresPublics.SelectedItem = liste;
        }
        
        [Given(@"Je sélectionne la valeur '(.*)' dans cbxLivresRayons")]
        public void GivenJeSelectionneLaValeurDansCbxLivresRayons(string liste)
        {
            ComboBox cbxLivresRayons = (ComboBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["cbxLivresRayons"];
            cbxLivresRayons.SelectedItem = liste;
        }
        
        [Given(@"Je saisis la valeur '(.*)' dans txbLivresTitreRecherche")]
        public void GivenJeSaisisLaValeurDansTxbLivresTitreRecherche(string titre)
        {
            TabControl tabonglet = (TabControl)frmMediatek.Controls["tabControl"];
            frmMediatek.Visible = true;
            tabonglet.SelectedTab = (TabPage)tabonglet.Controls["tabLivres"];
            TextBox txbLivresTitreRecherche = (TextBox)frmMediatek.Controls["tabControl"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresTitreRecherche"];

            txbLivresTitreRecherche.Text = titre;
        }
        
        [When(@"Je clique sur le bouton Rechercher")]
        public void WhenJeCliqueSurLeBoutonRechercher()
        {
            Button btnLivresNumRecherche = (Button)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["btnLivresNumRecherche"];
            frmMediatek.Visible = true;
            btnLivresNumRecherche.PerformClick();
        }
        
      
        [Then(@"Les informations détaillées affichent le titre '(.*)'")]
        public void ThenLesInformationsDetailleesAffichentLeTitre(string titreAttendu)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            string titreObtenu = txbLivresTitre.Text;
            Assert.AreEqual(titreAttendu, titreObtenu);

        }

      
        
        [Then(@"(.*) livre retourné")]
        public void ThenLivreRetourne(int nombreAttendu)
        {
            DataGridView dgvLivresListe = (DataGridView)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["dgvLivresListe"];
            int nombreObtenu = dgvLivresListe.Rows.Count;
            Assert.AreEqual(nombreAttendu, nombreObtenu);
        }
        
        [Then(@"(.*) livres obtenus")]
        public void ThenLivresObtenus(string listeAttendue)
        {
            DataGridView dgvLivresListe = (DataGridView)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["cbxLivresRayons"];
            int listeObtenue = dgvLivresListe.Rows.Count;
            Assert.AreEqual(listeAttendue, listeObtenue);
        }
        
        [Then(@"(.*) livre obtenu : Philippe Masson : Catastrophe au Brésil")]
        public void ThenLivreObtenuPhilippeMassonCatastropheAuBresil(string titre)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            string titreObtenu = txbLivresTitre.Text;
            Assert.AreEqual(titre, titreObtenu);
        }
    }
}
