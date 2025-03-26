
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Genre : hérite de Categorie
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id du genre</param>
        /// <param name="libelle">libelle du genre</param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
