
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Rayon (rayon de classement du document) hérite de Categorie
    /// </summary>
    public class Rayon : Categorie
    {
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="libelle">libelle du rayon</param>
        public Rayon(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
