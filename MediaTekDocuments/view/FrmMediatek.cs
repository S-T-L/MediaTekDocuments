using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
  
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
       
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();


        private readonly Utilisateur utilisateurAuth;


        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            this.utilisateurAuth = utilisateur;
            // Vérifier les droits d'accès avant d'initialiser les composants
            if (utilisateurAuth.Service.Libelle == "culture")
            {
                MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application.",
                                "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }
            InitializeComponent();
            this.controller = new FrmMediatekController();

            ConfigurerAcces();


        }

        /// <summary>
        /// Vérifie les abonnements expirants et affiche l'alerte si nécessaire
        /// </summary>
        private void AfficherAlerteAbonnementSiNecessaire()
        {
            // Récupérer les abonnements expirants depuis le contrôleur
            List<Abonnement> abonnementsExpirants = controller.GetAbonnementsExpirants();
            List<Revue> lesAbonnementsRevues = controller.GetAllRevues();

            // S'il y a des abonnements expirants, afficher l'alerte
            if (abonnementsExpirants.Any())
            {
                FrmAlerteAbonnement frmAlerteAbonnement = new FrmAlerteAbonnement(abonnementsExpirants, lesAbonnementsRevues);
                frmAlerteAbonnement.ShowDialog();
            }
        }
        /// <summary>
        /// Visibilité des éléments selon l'utilisateur connecté et son service
        /// </summary>
        private void ConfigurerAcces()
        {
            string libelleService = utilisateurAuth.Service.Libelle;

            if (libelleService == "prêt")
            {
                tabOngletsApplication.TabPages.Remove(tabCommandeLivre);
                tabOngletsApplication.TabPages.Remove(tabCommandeDVD);
                tabOngletsApplication.TabPages.Remove(tabCommandeRevue);
                tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            }

        }
        /// <summary>
        /// Affichage de l'alerte pour les abonnements selon l'utilisateur authentifié
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMediatek_Shown(object sender, EventArgs e)
        {


            // Vérifier les abonnements si l'utilisateur est autorisé
            if (utilisateurAuth.Service.Libelle == "administratif" || utilisateurAuth.Service.Libelle == "administrateur")
            {
                AfficherAlerteAbonnementSiNecessaire();
            }
        }


        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        private void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Rempli le dgvDvdListe
        /// </summary>
        /// <param name="Dvd">liste dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvd)
        {
            bdgDvdListe.DataSource = Dvd;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commande de livres

        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();
        private readonly BindingSource bdgDocCommandeSuivi = new BindingSource();
        private readonly BindingSource bdgLivreCommandesListe = new BindingSource();
        private Dictionary<string, bool> ordreTri = new Dictionary<string, bool>();



        /// <summary>
        /// Clic de l'utilisateur sur l'onglet commandes de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboSuivi(controller.GetAllSuivis(), bdgDocCommandeSuivi, cbxEtatCommandeLivre);
            ModifEnCoursComLivre(false, false);
        }
        /// <summary>
        /// gère l'affichage selon si modification en cours et si commande trouvée
        /// </summary>
        /// <param name="modif"></param>
        /// <param name="commandeTrouve"></param>
        private void ModifEnCoursComLivre(bool modif, bool commandeTrouve)
        {

            grpSuiviCommandeLivre.Visible = modif && commandeTrouve;
            grpCommandeLivre.Visible = modif;
            dgvListeLivresCom.Visible = modif;
            cbxEtatCommandeLivre.SelectedIndex = 0;
            

        }

        /// <summary>
        /// Remplissage du combo avec les libellés de suivi
        /// </summary>
        /// <param name="lesSuivis">liste les suivis</param>
        /// <param name="bdg">bindingSource</param>
        /// <param name="cbx">combobox</param>
        public void RemplirComboSuivi(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Remplit le datagrid qui liste les commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandesDoc">liste de commandes de document</param>
        private void RemplirDgvCommandesLivres(List<CommandeDocument> commandesDoc)
        {
            if (commandesDoc != null)
            {
                bdgLivreCommandesListe.DataSource = commandesDoc;
                dgvListeLivresCom.DataSource = bdgLivreCommandesListe;

                dgvListeLivresCom.Columns["id"].Visible = false;
                dgvListeLivresCom.Columns["idLivreDvd"].Visible = false;
                dgvListeLivresCom.Columns["idSuivi"].Visible = false;
                dgvListeLivresCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeLivresCom.Columns["dateCommande"].DisplayIndex = 0;
                dgvListeLivresCom.Columns["montant"].DisplayIndex = 1;
                dgvListeLivresCom.RowHeadersVisible = false;


                // Définir les entêtes des colonnes
                dgvListeLivresCom.Columns["dateCommande"].HeaderText = "Date de la Commande";
                dgvListeLivresCom.Columns["montant"].HeaderText = "Montant Total";
                dgvListeLivresCom.Columns["libelleSuivi"].HeaderText = "Statut de la Commande";
                dgvListeLivresCom.Columns["nbExemplaire"].HeaderText = "Nombre d'exemplaire";

            }
            else
            {
                dgvListeLivresCom.DataSource = null;
            }
        }


        /// <summary>
        /// recherche par numéro de livre saisi par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercherCommandeLivre_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txbNumDocCommandeLivre.Text))

            {
                // recherche un livre correspondant à l'id saisi
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbNumDocCommandeLivre.Text));
                if (livre != null)
                {
                    //affichage de ses informations
                    AfficheInfosLivreRecherche(livre);
                    dgvListeLivresCom.ClearSelection();
                    cbxEtatCommandeLivre.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }


        /// <summary>
        /// Affichage des informations du livre recherché
        /// </summary>
        /// <param name="livre">Objet livre contenant les informations de celui ci</param>
        private void AfficheInfosLivreRecherche(Livre livre)
        {
            txbNumDocCommandeLivre.Text = livre.Id;
            txbCommandeLivreTitre.Text = livre.Titre;
            txbCommandeLivreAuteur.Text = livre.Auteur;
            txbCommandeLivreCodeISBN.Text = livre.Isbn;
            txbCommandeLivreCollection.Text = livre.Collection;
            txbCommandeLivreGenre.Text = livre.Genre;
            txbCommandeLivrePublic.Text = livre.Public;
            txbCommandeLivreRayon.Text = livre.Rayon;
            txbCommandeLivreUrlImage.Text = livre.Image;
            string image = livre.Image;
            try
            {
                pcbCommandeLivreImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeLivreImage = null;
            }
            // affiche la liste des commandes du livre
            AfficheLivreCommandes();
        }

        /// <summary>
        /// Récupère et affiche les commandes d'un livre
        /// </summary>
        private void AfficheLivreCommandes()
        {
            string idDocument = txbNumDocCommandeLivre.Text;
            //Appel au controleur pour récupérer les commandes liées à ce livre
            lesCommandesDocument = controller.GetCommandesDocument(idDocument);
            RemplirDgvCommandesLivres(lesCommandesDocument);
            // Si des commandes existent
            if (lesCommandesDocument.Count > 0)
            {
                ModifEnCoursComLivre(true, true);
            }
            else
            {
                MessageBox.Show("Aucune commande trouvée pour ce livre");
                ModifEnCoursComLivre(true, false);
            }
        }

        /// <summary>
        /// vérifie si l'identifiant renseigné existe déja
        /// </summary>
        /// <param name="id">identifiant de la commande</param>
        /// <returns>true si la commande existe, sinon false</returns>
        private bool CommandeExistante(string id)
        {
            List<Commande> commandesExistantes = controller.GetAllCommandes();
            if (commandesExistantes.Exists(c => c.Id == id))
            {
                MessageBox.Show("Une commande avec ce numéro existe déjà", "Erreur");
                //si commande existe
                return true;
            }
            //si elle n'existe pas
            return false;
        }

        /// <summary>
        /// Vérifie que les champs sont correctement renseignés
        /// </summary>
        /// <param name="commandeId"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        /// <returns>True si tous les champs sont valides, sinon False</returns>
        private bool VerifierChampsCommande(string commandeId, string nbExemplaire, string montant, DateTime dateCommande)
        {
            // Vérification des champs vides
            if (string.IsNullOrWhiteSpace(commandeId))
            {
                MessageBox.Show("Numéro de commande obligatoire", "Information");
                return false;
            }

            if (string.IsNullOrWhiteSpace(nbExemplaire))
            {
                MessageBox.Show("Le nombre d'exemplaires est obligatoire", "Information");
                return false;
            }

            if (string.IsNullOrWhiteSpace(montant))
            {
                MessageBox.Show("Le montant est obligatoire", "Information");
                return false;
            }

            // Vérification des formats numériques
            if (!int.TryParse(nbExemplaire, out int nbEx) || nbEx <= 0)
            {
                MessageBox.Show("Le nombre d'exemplaires doit être un nombre entier ", "Information");
                return false;
            }

            if (!double.TryParse(montant, out double mt) || mt <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre décimal positif", "Information");
                return false;
            }
            // Vérification de la date
            if (dateCommande <= DateTime.Now.Date)
            {
                MessageBox.Show("La date de commande ne peut pas être inférieure à la date d'aujourd'hui");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Ajout d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutCommandeLivre_Click(object sender, EventArgs e)
        {
            // vérification des champs remplis
            if (!VerifierChampsCommande(txbNumCommandeLivre.Text, txbCommandeLivreNbEx.Text, txbCommandeLivreMontant.Text, dtpCommandeLivreDate.Value))
            {
                return;
            }
            // Vérification du suivi
            Suivi suivi = (Suivi)cbxEtatCommandeLivre.SelectedItem;
            if (suivi.Id != 1)
            {
                MessageBox.Show("Une nouvelle commande ne peut être créée qu'avec un état en cours");
                return;
            }
            try
            {
                string id = txbNumCommandeLivre.Text;
                string idLivreDvd = txbNumDocCommandeLivre.Text;

                // Si numero de commande existant déja
                if (CommandeExistante(id))
                {
                    txbAbonnementNumCommande.Focus();
                    return;
                }
                int nbExemplaire = int.Parse(txbCommandeLivreNbEx.Text);
                DateTime dateCommande = dtpCommandeLivreDate.Value;
                int idSuivi = suivi.Id;
                string libelleSuivi = suivi.Libelle;
                double montant = double.Parse(txbCommandeLivreMontant.Text);
                //Création
                CommandeDocument commandeDocument = new CommandeDocument(
                    id, dateCommande, montant, nbExemplaire,
                    idLivreDvd, idSuivi, libelleSuivi
                );
                if (controller.CreerCommandeDocument(commandeDocument))
                {
                    AfficheLivreCommandes();
                    MessageBox.Show("La commande numéro " + commandeDocument.Id + " pour le livre " + lesLivres.Find(o => o.Id == commandeDocument.IdLivreDvd).Titre + " a bien été prise en compte ");
                    grpCommandeLivre.Controls.OfType<TextBox>().ToList().ForEach(txt => txt.Clear());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veuillez renseigner des valeurs correctes", "Information");
                ReinitialiserChampsComLivre();
            }
        }





        /// <summary>
        /// Réinitialise les champs de saisie
        /// </summary>
        private void ReinitialiserChampsComLivre()
        {
            txbNumCommandeLivre.Text = "";
            txbCommandeLivreNbEx.Text = "";
            txbCommandeLivreMontant.Text = "";
            txbNumCommandeLivre.Focus();
            
            //utilisation de linq pour filtrer directement tous les txtbox dans grpCommandeLivreInfo
            grpCommandeLivreInfo.Controls.OfType<TextBox>().ToList().ForEach(txt => txt.Clear());
        }
        /// <summary>
        /// Suppression d'une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeLivre_Click(object sender, EventArgs e)
        {

            CommandeDocument commandeDocument = (CommandeDocument)bdgLivreCommandesListe[bdgLivreCommandesListe.Position];
            if (dgvListeLivresCom.CurrentCell != null)
            {
                // vérifie si la commande est livrée ou réglée
                if (commandeDocument.IdSuivi > 2)
                    MessageBox.Show("Une commande livrée ou réglée ne peut etre supprimée");
                else if (MessageBox.Show("Voulez vous supprimer la commande " + commandeDocument.Id + " pour le livre " + lesLivres.Find(o => o.Id == commandeDocument.IdLivreDvd).Titre + " ?",
                    "Validation suppresion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //appel au controleur
                    if (controller.SupprimerCommandeDocument(commandeDocument.Id))
                    {
                        AfficheLivreCommandes();
                    }
                    else
                    {
                        MessageBox.Show("erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("la selection d'une commande est obligatoire");
            }
        }

        /// <summary>
        /// récupération de la commande sélectionnée dans dgvListeLivreCom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeLivresCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListeLivresCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivreCommandesListe[bdgLivreCommandesListe.Position];


                // Rendre visible le GroupBox de suivi
                grpSuiviCommandeLivre.Visible = true;

                // Récupérer la liste des suivis depuis le contrôleur
                List<Suivi> lesSuivis = controller.GetAllSuivis();

                // Sélectionner l'état actuel de la commande dans le ComboBox
                Suivi suiviActuel = lesSuivis.Find(s => s.Id == commandeDocument.IdSuivi);
                if (suiviActuel != null)
                {
                    cbxEtatCommandeLivre.Text = suiviActuel.ToString();
                }

            }
            else
            {
                // Cacher le GroupBox si aucune commande n'est sélectionnée
                grpSuiviCommandeLivre.Visible = false;
            }
        }


        /// <summary>
        /// Modification de l'état d'une commande si son suivi actuel le permet
        /// </summary>
        private void ModifierSuiviCommandeLivre()
        {
            // ligne sélectionnée
            if (dgvListeLivresCom.CurrentRow != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivreCommandesListe[bdgLivreCommandesListe.Position];
                //récupération du suivi sélectionné par l'utilisateur dans le cbx
                Suivi nouvelEtat = (Suivi)cbxEtatCommandeLivre.SelectedItem;

                //Vérification des états de suivi
                if (commandeDocument.IdSuivi >= 3)
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir en arrière.", "Modification impossible");
                    return;
                }

                if (nouvelEtat.Id == 4 && commandeDocument.IdSuivi != 3)
                {
                    MessageBox.Show("Une commande ne peut être réglée que si elle est livrée.", "Modification impossible");
                    return;
                }
                commandeDocument.IdSuivi = nouvelEtat.Id;

                if (controller.ModiferCommandeDocument(commandeDocument))
                {
                    MessageBox.Show("Suivi mis à jour avec succès !");
                    AfficheLivreCommandes();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du suivi.", "Erreur");
                }
            }
        }


        /// <summary>
        /// Clic sur le bouton modifier / Gestion de l'état de la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMAJSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            ModifierSuiviCommandeLivre();
        }


        /// <summary>
        /// Gestion des tris dans le dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeLivresCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeLivresCom.Columns[e.ColumnIndex].DataPropertyName;

            if (!ordreTri.ContainsKey(nomColonne))
                // tri ascendant par défaut
                ordreTri[nomColonne] = true;

            List<CommandeDocument> listeTriee;

            if (ordreTri[nomColonne])
                listeTriee = lesCommandesDocument.OrderBy(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            else
                listeTriee = lesCommandesDocument.OrderByDescending(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            //Inversion de l'ordre
            ordreTri[nomColonne] = !ordreTri[nomColonne];

            // Mise à jour du bdg et dgv
            bdgLivreCommandesListe.DataSource = listeTriee;
            dgvListeLivresCom.DataSource = bdgLivreCommandesListe;
        }
        /// <summary>
        /// Modification du texte dans txbNumDocCommandeLivre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbNumDocCommandeLivre_TextChanged(object sender, EventArgs e)
        {
            ModifEnCoursComLivre(false, false);
            ReinitialiserChampsComLivre();
        }


        #endregion

        #region Onglet Commande de dvd

        private readonly BindingSource bdgDvdCommandesListe = new BindingSource();


        /// <summary>
        /// Clic sur onglet Commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeDVD_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboSuivi(controller.GetAllSuivis(), bdgDocCommandeSuivi, cbxEtatCommandeDvd);
            ModifEnCoursComDvd(false, false);

        }

       /// <summary>
       /// Gestion de l'affichage selon si modification ou commande trouvée
       /// </summary>
       /// <param name="modif">modif </param>
       /// <param name="commandeTrouve">commande trouvée</param>
        private void ModifEnCoursComDvd(bool modif, bool commandeTrouve)
        {
            grpSuiviCommandeDvd.Visible = modif && commandeTrouve;
            grpCommandeDvd.Visible = modif;
            dgvListeDvdCom.Visible = modif;
        }

        /// <summary>
        /// Clic sur bouton rechercher / Affiche les infos du numéro de dvd renseigné par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCommandeDvd_Click(object sender, EventArgs e)
        {
            if (!txbNumDocRechercheDvd.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbNumDocRechercheDvd.Text));
                if (dvd != null)
                {
                    AfficheInfosDvdRecherche(dvd);

                }

            }
            else
            {
                MessageBox.Show("Numéro introuvable");
            }
        }

      /// <summary>
      /// Affiche les informations du dvd recherché
      /// </summary>
      /// <param name="dvd">objet dvd</param>
        private void AfficheInfosDvdRecherche(Dvd dvd)
        {
            txbCommandeDvdNumDoc.Text = dvd.Id;
            txbCommandeDvdDuree.Text = dvd.Duree.ToString();
            txbCommandeDvdTitre.Text = dvd.Titre;
            txbCommandeDvdReal.Text = dvd.Realisateur;
            txbCommandeDvdSynop.Text = dvd.Synopsis;
            txbCommandeDvdGenre.Text = dvd.Genre;
            txbCommandeDvdPublic.Text = dvd.Public;
            txbCommandeDvdRayon.Text = dvd.Rayon;
            string image = dvd.Image;
            try
            {
                pcbCommandeDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeDvdImage = null;
            }
            // affiche la liste des commandes du dvd
            AfficheDvdCommandes();

        }

        /// <summary>
        /// Récupère et affiche les commandes d'un dvd
        /// </summary>
        private void AfficheDvdCommandes()
        {
            string idDocument = txbNumDocRechercheDvd.Text;
            lesCommandesDocument = controller.GetCommandesDocument(idDocument);
            RemplirCommandesDvd(lesCommandesDocument);
            if (lesCommandesDocument.Count > 0)
            {
                ModifEnCoursComDvd(true, true);
            }
            else
            {
                MessageBox.Show("Aucune commande trouvée pour ce dvd");
                ModifEnCoursComDvd(true, false);
            }

        }

        /// <summary>
        /// Remplit le datagrid qui liste les commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandesDoc">liste de commandes de document</param>
        private void RemplirCommandesDvd(List<CommandeDocument> commandesDoc)
        {
            if (commandesDoc != null)
            {
                bdgDvdCommandesListe.DataSource = commandesDoc;
                dgvListeDvdCom.DataSource = bdgDvdCommandesListe;
                dgvListeDvdCom.Columns["id"].Visible = false;
                dgvListeDvdCom.Columns["idLivreDvd"].Visible = false;
                dgvListeDvdCom.Columns["idSuivi"].Visible = false;
                dgvListeDvdCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeDvdCom.Columns["dateCommande"].DisplayIndex = 0;
                dgvListeDvdCom.Columns["montant"].DisplayIndex = 1;
                dgvListeDvdCom.RowHeadersVisible = false;

                // Définir les entêtes des colonnes
                dgvListeDvdCom.Columns["dateCommande"].HeaderText = "Date de la Commande";
                dgvListeDvdCom.Columns["montant"].HeaderText = "Montant Total";
                dgvListeDvdCom.Columns["libelleSuivi"].HeaderText = "Statut de la Commande";
                dgvListeDvdCom.Columns["nbExemplaire"].HeaderText = "Nombre d'exemplaire";
            }
            else
            {
                dgvListeDvdCom.DataSource = null;
            }
        }


        /// <summary>
        /// Clic sur bouton Ajouter / Ajout d'une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutComDvd_Click(object sender, EventArgs e)
        {
            if (!VerifierChampsCommande(txbNumCommandeDvd.Text, txbCommandeDvdNbEx.Text, txbCommandeDvdMontant.Text, dtpCommandeDvdDate.Value))
            {
                return;

            }

            try
            {
                string id = txbNumCommandeDvd.Text;
                string idLivreDvd = txbNumDocRechercheDvd.Text;


                // Si numero de commande existant déja
                if (CommandeExistante(id))
                {
                    txbAbonnementNumCommande.Focus();
                    return;

                }


                int nbExemplaire = int.Parse(txbCommandeDvdNbEx.Text);
                DateTime dateCommande = dtpCommandeDvdDate.Value;
                Suivi suivi = (Suivi)cbxEtatCommandeDvd.SelectedItem;
                int idSuivi = suivi.Id;
                string libelleSuivi = suivi.Libelle;
                double montant = double.Parse(txbCommandeDvdMontant.Text);

                CommandeDocument commandeDocument = new CommandeDocument(
                    id, dateCommande, montant, nbExemplaire,
                    idLivreDvd, idSuivi, libelleSuivi
                );

                if (controller.CreerCommandeDocument(commandeDocument))
                {
                    AfficheDvdCommandes();
                    MessageBox.Show("La commande numéro " + commandeDocument.Id + " pour le dvd " + lesDvd.Find(o => o.Id == commandeDocument.IdLivreDvd).Titre + " a bien été prise en compte ");
                    grpCommandeDvd.Controls.OfType<TextBox>().ToList().ForEach(txt => txt.Clear());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veuillez renseigner des valeurs correctes", "Information");
                ReinitialiserChampsComDvd();
            }
        }

        /// <summary>
        /// Clic sur bouton modifier / Modification de l'état de suivi d'une commande dvd
        /// </summary>
        private void ModifierSuiviCommandeDvd()
        {
            if (dgvListeDvdCom.CurrentRow != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgDvdCommandesListe[bdgDvdCommandesListe.Position];
                Suivi nouvelEtat = (Suivi)cbxEtatCommandeDvd.SelectedItem;


                // Vérification des règles métier
                if (commandeDocument.IdSuivi >= 3)
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir en arrière.", "Modification impossible");
                    return;
                }

                if (nouvelEtat.Id == 4 && commandeDocument.IdSuivi != 3)
                {
                    MessageBox.Show("Une commande ne peut être réglée que si elle est livrée.", "Modification impossible");
                    return;
                }
                commandeDocument.IdSuivi = nouvelEtat.Id;
                // Envoi de la mise à jour au contrôleur
                if (controller.ModiferCommandeDocument(commandeDocument))
                {
                    MessageBox.Show("Suivi mis à jour avec succès !");
                    AfficheDvdCommandes(); // Rafraîchissement de la liste
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du suivi.", "Erreur");
                }
            }
        }
        /// <summary>
        /// Clic sur bouton MAJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMAJSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            ModifierSuiviCommandeDvd();
        }
        /// <summary>
        /// Tri des colonnes de dgvListeDvdCom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeDvdCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeDvdCom.Columns[e.ColumnIndex].DataPropertyName;

            if (!ordreTri.ContainsKey(nomColonne))
                // tri ascendant par défaut
                ordreTri[nomColonne] = true;

            List<CommandeDocument> listeTriee;

            if (ordreTri[nomColonne])
                listeTriee = lesCommandesDocument.OrderBy(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            else
                listeTriee = lesCommandesDocument.OrderByDescending(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            //Inversion de l'ordre
            ordreTri[nomColonne] = !ordreTri[nomColonne];

            // Mise à jour du bdg et dgv
            bdgDvdCommandesListe.DataSource = listeTriee;
            dgvListeDvdCom.DataSource = bdgDvdCommandesListe;
        }

        /// <summary>
        /// Réinitialise les champs de saisie
        /// </summary>
        /// 
        private void ReinitialiserChampsComDvd()
        {
            txbNumCommandeDvd.Text = "";
            txbCommandeDvdNbEx.Text = "";
            txbCommandeDvdMontant.Text = "";
            txbNumCommandeDvd.Focus();
            //utilisation de linq pour filtrer directement tous les txtbox dans grpCommandeLivreInfo
            grpCommandeDvdInfo.Controls.OfType<TextBox>().ToList().ForEach(txt => txt.Clear());
        }
        /// <summary>
        /// Clic sur bouton Supprimer une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCommandeDvd_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDocument = (CommandeDocument)bdgDvdCommandesListe[bdgDvdCommandesListe.Position];
            if (dgvListeDvdCom.CurrentCell != null)
            {
                if (commandeDocument.IdSuivi > 2)
                    MessageBox.Show("Une commande livrée ou réglée ne peut etre supprimée");
                else if (MessageBox.Show("Voulez vous supprimer la commande " + commandeDocument.Id + " pour le dvd " + lesDvd.Find(o => o.Id == commandeDocument.IdLivreDvd).Titre + " ?",
                    "Validation suppresion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.SupprimerCommandeDocument(commandeDocument.Id))
                    {

                        AfficheDvdCommandes();

                    }
                    else
                    {
                        MessageBox.Show("erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("la selection d'une commande est obligatoire");
            }
        }
        /// <summary>
        /// Modification du texte dans txbNumDocRechercheDvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbNumDocRechercheDvd_TextChanged(object sender, EventArgs e)
        {
            ModifEnCoursComDvd(false, false);
            ReinitialiserChampsComDvd();
        }
        #endregion


        #region Onglet Commande de revues

        private List<Revue> lesRevuesAbonnement = new List<Revue>();
        private readonly BindingSource bdgAbonnementRevue = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();

        /// <summary>
        ///Clic sur l'onglet commande de revue 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeRevue_Enter(object sender, EventArgs e)
        {
            lesRevuesAbonnement = controller.GetAllRevues();
            ModifEnCoursComRevue(false);
        }
        /// <summary>
        /// Recherche d'une revue par son numéro, clic sur btnRechercher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercherAbonnement_Click(object sender, EventArgs e)
        {
            if (!txbAbonnementNumRevue.Text.Equals(""))
            {

                Revue revue = lesRevuesAbonnement.Find(x => x.Id.Equals(txbAbonnementNumRevue.Text));
                if (revue != null)
                {
                    AfficheInfosRevueRecherche(revue);
                    ModifEnCoursComRevue(true);

                }

            }
            else
            {
                MessageBox.Show("Numéro introuvable");
            }
        }


       /// <summary>
       /// Affiche les infos de la revue recherchée
       /// </summary>
       /// <param name="revue"></param>objet revue
        private void AfficheInfosRevueRecherche(Revue revue)
        {
            txbAbonnementTitre.Text = revue.Titre;
            txbAbonnementPeriod.Text = revue.Periodicite;
            txbAbonnementDelai.Text = revue.DelaiMiseADispo.ToString();
            txbAbonnementGenre.Text = revue.Genre;
            txbAbonnementPublic.Text = revue.Public;
            txbAbonnementRayon.Text = revue.Rayon;

            // affiche la liste des commandes du dvd
            AfficheRevueCommandes();

        }

        /// <summary>
        /// Récupère et affiche les commandes d'une revue
        /// </summary>
        private void AfficheRevueCommandes()
        {
            string idRevue = txbAbonnementNumRevue.Text;
            lesAbonnements = controller.GetAbonnementsRevue(idRevue);
            RemplirCommandesRevue(lesAbonnements);

        }

        /// <summary>
        /// Gestion de l'affichage : true modif en cours
        /// </summary>
        /// <param name="modif"></param>
        private void ModifEnCoursComRevue(bool modif)
        {
            grpCommandeRevue.Visible = modif;
            dgvListeAbonnements.Visible = modif;
        }

        /// <summary>
        /// rempli le dgv avec la liste des commandes de revues
        /// </summary>
        /// <param name="lesAbonnements">liste les abonnements</param>
        private void RemplirCommandesRevue(List<Abonnement> lesAbonnements)
        {
            if (lesAbonnements != null)
            {
                bdgAbonnementRevue.DataSource = lesAbonnements;
                dgvListeAbonnements.DataSource = bdgAbonnementRevue;

                dgvListeAbonnements.Columns["idRevue"].Visible = false;
                dgvListeAbonnements.Columns["id"].Visible = false;

                dgvListeAbonnements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvListeAbonnements.Columns["dateCommande"].DisplayIndex = 0;
                dgvListeAbonnements.Columns["montant"].DisplayIndex = 1;


            }
            else
            {
                dgvListeAbonnements.DataSource = null;
            }
        }

        /// <summary>
        /// Vérification du contenu des champs obligatoires pour la saisie d'une nouvelle commande
        /// </summary>
        /// <param name="idCommande">id de la commande</param>
        /// <param name="montantText">montant</param>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <returns>true si tous les champs sont valides sinon false</returns>
        public static bool VerifierChampsCommandeRevue(string idCommande, string montantText, DateTime dateCommande, DateTime dateFinAbonnement)
        {
            if (string.IsNullOrWhiteSpace(idCommande))
            {
                MessageBox.Show("Numéro de commande obligatoire", "Information");
                return false;
            }

            if (string.IsNullOrWhiteSpace(montantText))
            {
                MessageBox.Show("Le montant est obligatoire", "Information");
                return false;
            }

            if (!double.TryParse(montantText, out double montant) || montant <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre décimal positif.", "Erreur");
                return false;
            }

            if (dateFinAbonnement < dateCommande)
            {
                MessageBox.Show("La date de fin d'abonnement ne peut pas être antérieure à la date de commande.", "Erreur");
                return false;
            }

            if (dateCommande <= DateTime.Today)
            {
                MessageBox.Show("La date de commande ne peut pas être inférieure à la date d'aujourd'hui");
                return false;
            }

            return true;
        }




        /// <summary>
        /// Clic sur btnAjoutAbonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutAbonnement_Click(object sender, EventArgs e)
        {
            if (!VerifierChampsCommandeRevue(txbAbonnementNumCommande.Text, txbAbonnementMontant.Text, dtpAbonnementDate.Value, dtpDateFinAbonnement.Value))
            {
                return;

            }

            try
            {
                string id = txbAbonnementNumCommande.Text;

                string idRevue = txbAbonnementNumRevue.Text;


                // Si numero de commande existant déja
                if (CommandeExistante(id))
                {
                    txbAbonnementNumCommande.Focus();
                    return;

                }
                DateTime dateCommande = dtpAbonnementDate.Value;
                DateTime dateFin = dtpDateFinAbonnement.Value;

                double montant = double.Parse(txbAbonnementMontant.Text);

                Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFin, idRevue);



                if (controller.CreerAbonnement(abonnement))
                {
                    AfficheRevueCommandes();
                    MessageBox.Show("La commande numéro " + abonnement.Id + " pour la revue" + lesRevuesAbonnement.Find(o => o.Id == abonnement.IdRevue).Titre + " a bien été prise en compte ");
                    grpCommandeRevue.Controls.OfType<TextBox>().ToList().ForEach(txt => txt.Clear());
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veuillez renseigner des valeurs correctes", "Information");
                ReinitialiserChampsComLivre();
            }
        }
        /// <summary>
        /// Suppression d'une commande de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprAbonnement_Click(object sender, EventArgs e)
        {
            if (dgvListeAbonnements.CurrentCell != null)
            {
                // Récupère l'abonnement sélectionné
                Abonnement abonnement = (Abonnement)bdgAbonnementRevue[bdgAbonnementRevue.Position];
                // récupération de la liste des exemplaires associés
                List<Exemplaire> exemplairesRevue = controller.GetExemplairesRevue(abonnement.IdRevue);

                //Vérifie si un exemplaire est rattaché sur la période d'abonnement
                bool exemplaireTrouve = exemplairesRevue.Exists(exemplaire =>
     abonnement.ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaire.DateAchat));

                if (exemplaireTrouve)
                {
                    MessageBox.Show("Impossible de supprimer cet abonnement car des exemplaires y sont rattachés.", "Erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Demande de confirmation avant suppression
                    if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer l'abonnement à la revue " +
                                        lesRevuesAbonnement.Find(r => r.Id == abonnement.IdRevue).Titre + " ?",
                                        "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (controller.SupprimerAbonnementRevue(abonnement.Id))
                        {
                            AfficheRevueCommandes();
                            MessageBox.Show("L'abonnement a été supprimé.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression de l'abonnement.", "Erreur");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un abonnement à supprimer.", "Information");
            }
        }
        /// <summary>
        /// Modification du texte dans txbAbonnementNumRevue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbAbonnementNumRevue_TextChanged(object sender, EventArgs e)
        {
            ModifEnCoursComRevue(false);
        }
        /// <summary>
        /// tri des colonnes de dgvListeAbonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeAbonnements_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nomColonne = dgvListeAbonnements.Columns[e.ColumnIndex].DataPropertyName;

            if (!ordreTri.ContainsKey(nomColonne))
                // tri ascendant par défaut
                ordreTri[nomColonne] = true;

            List<Abonnement> listeTriee;

            if (ordreTri[nomColonne])
                listeTriee = lesAbonnements.OrderBy(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            else
                listeTriee = lesAbonnements.OrderByDescending(o => o.GetType().GetProperty(nomColonne).GetValue(o)).ToList();
            //Inversion de l'ordre
            ordreTri[nomColonne] = !ordreTri[nomColonne];

            // Mise à jour du bdg et dgv
            bdgAbonnementRevue.DataSource = listeTriee;
            dgvListeAbonnements.DataSource = bdgAbonnementRevue.DataSource = listeTriee;

        }

        #endregion
    }
}









