using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    class FrmAuthentificationController
    {
        /// <summary>
        /// accès aux données
        /// </summary>
        private readonly Access access;

        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }

        public Utilisateur GetAuthentification(string loginUtilisateur, string password)
        {
            // Étape 1 : Récupérer l'utilisateur à partir de la méthode GetAuthentification de la classe Access
            Utilisateur utilisateur = access.GetAuthentification(loginUtilisateur);

            // Si aucun utilisateur n'est trouvé, retour false
            if (utilisateur == null)
            {
                return null;
            }

            // Vérification du mot de passe haché
            string passwordHache = HashPassword(password);
            if (passwordHache != utilisateur.Password)
            {
                return null; // Échec de l'authentification
            }

           

            return utilisateur;
        }

            /// <summary>
            /// Méthode pour hasher un mot de passe avec SHA-256
            /// </summary>
            private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Calculer le hash du mot de passe
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convertir le tableau de bytes en une chaîne hexadécimale
                StringBuilder builder = new StringBuilder();
                foreach (byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }

                return builder.ToString();
            }
        }


    }
}
