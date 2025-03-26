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
        /// Nouvelle instance
        /// </summary>
        /// <param name="abonnementsExpirants">abonnements expirants</param>
        public FrmAlerteAbonnement(List<Abonnement> abonnementsExpirants)
        {
            InitializeComponent();
           
            bdgAlerteAbo.DataSource = abonnementsExpirants;
            dgvFinAbonnement.DataSource = bdgAlerteAbo;
            dgvFinAbonnement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvFinAbonnement.Columns["dateFinAbonnement"].DisplayIndex = 1;
            dgvFinAbonnement.Columns[0].HeaderCell.Value = "Date de fin d'abonnement";
            dgvFinAbonnement.Columns[1].HeaderCell.Value = "Identitifiant";
            dgvFinAbonnement.Columns[2].HeaderCell.Value = "Titre de la Revue";

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

