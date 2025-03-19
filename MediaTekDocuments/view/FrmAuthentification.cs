using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form

    {
        private readonly FrmAuthentificationController controller;
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        private void btnSeConnecter_Click(object sender, EventArgs e)
        {
            // Récupération de l'utilisateur authentifié
            Utilisateur utilisateur = controller.GetAuthentification(txbNomUtilisateur.Text, txbPassword.Text);

            // Vérification si l'utilisateur est bien authentifié
            if (utilisateur != null)
            {
                this.Visible = false; // Masquer la fenêtre d'authentification

                // Ouvrir l'application principale en passant l'utilisateur
                FrmMediatek mediatek = new FrmMediatek(utilisateur);
                mediatek.Show();
            }
            else
            {
                MessageBox.Show("Erreur sur le login ou le mot de passe.", "Authentification échouée", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txbNomUtilisateur_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
