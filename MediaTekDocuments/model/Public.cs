
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Public (public concerné par le document) hérite de Categorie
    /// </summary>
    public class Public : Categorie
    {
        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id </param>
        /// <param name="libelle">libelle du public</param>
        public Public(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
