using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
   
    /// <summary>
    /// Fenêtre d'alerte pour les abonnements expirants
    /// </summary>
    public partial class FrmAlerteAbonnement : Form
    {

        
        private BindingSource bdgAlerteAbo = new BindingSource();

        /// <summary>
        /// Formulaire Alerte fin d'abonnements
        /// </summary>
        /// <param name="abonnementsExpirants"></param>
        /// <param name="lesRevues"></param>
        public FrmAlerteAbonnement(List<Abonnement> abonnementsExpirants, List<Revue> lesRevues)
        {
            InitializeComponent();

            // Récupérer les abonnements expirants
            var abonnementsRevueExp = abonnementsExpirants.Select(abonnement => new
            {
                abonnement.DateFinAbonnement,
                abonnement.IdRevue,
                TitreRevue = lesRevues.Find(revue => revue.Id == abonnement.IdRevue)?.Titre
            }).ToList();

            // Lier les abonnements et les titres de revue au DataGridView
            bdgAlerteAbo.DataSource = abonnementsRevueExp;
            dgvFinAbonnement.DataSource = bdgAlerteAbo;
            dgvFinAbonnement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvFinAbonnement.Columns["idRevue"].Visible = false;


            // Définir les en-têtes de colonnes
            dgvFinAbonnement.Columns["DateFinAbonnement"].HeaderCell.Value = "Date de fin d'abonnement";
            dgvFinAbonnement.Columns["TitreRevue"].HeaderCell.Value = "Titre de la Revue";

        }

      
        /// <summary>
        /// Clic sur btnAcces pour quitter la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccesAppli_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }












}

