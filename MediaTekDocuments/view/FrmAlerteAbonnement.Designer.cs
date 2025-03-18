
namespace MediaTekDocuments.view
{
    partial class FrmAlerteAbonnement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvFinAbonnement = new System.Windows.Forms.DataGridView();
            this.lblAbo = new System.Windows.Forms.Label();
            this.btnAccesAppli = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnement)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFinAbonnement
            // 
            this.dgvFinAbonnement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinAbonnement.Location = new System.Drawing.Point(12, 72);
            this.dgvFinAbonnement.Name = "dgvFinAbonnement";
            this.dgvFinAbonnement.RowHeadersWidth = 51;
            this.dgvFinAbonnement.RowTemplate.Height = 24;
            this.dgvFinAbonnement.Size = new System.Drawing.Size(377, 172);
            this.dgvFinAbonnement.TabIndex = 0;
            // 
            // lblAbo
            // 
            this.lblAbo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbo.Location = new System.Drawing.Point(24, 9);
            this.lblAbo.Name = "lblAbo";
            this.lblAbo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAbo.Size = new System.Drawing.Size(539, 46);
            this.lblAbo.TabIndex = 1;
            this.lblAbo.Text = "Abonnements se terminant dans moins de 30 jours";
            this.lblAbo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAccesAppli
            // 
            this.btnAccesAppli.Location = new System.Drawing.Point(436, 115);
            this.btnAccesAppli.Name = "btnAccesAppli";
            this.btnAccesAppli.Size = new System.Drawing.Size(144, 74);
            this.btnAccesAppli.TabIndex = 2;
            this.btnAccesAppli.Text = "Accéder à l\'application";
            this.btnAccesAppli.UseVisualStyleBackColor = true;
            this.btnAccesAppli.Click += new System.EventHandler(this.btnAccesAppli_Click);
            // 
            // FrmAlerteAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 277);
            this.Controls.Add(this.btnAccesAppli);
            this.Controls.Add(this.lblAbo);
            this.Controls.Add(this.dgvFinAbonnement);
            this.Name = "FrmAlerteAbonnement";
            this.Text = "FrmAlerteAbonnement";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFinAbonnement;
        private System.Windows.Forms.Label lblAbo;
        private System.Windows.Forms.Button btnAccesAppli;
    }
}