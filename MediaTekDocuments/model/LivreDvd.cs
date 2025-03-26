
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier LivreDvd hérite de Document
    /// </summary>
    public abstract class LivreDvd : Document
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="titre">titre</param>
        /// <param name="image">image</param>
        /// <param name="idGenre">idGenre</param>
        /// <param name="genre">genre</param>
        /// <param name="idPublic">idPublic</param>
        /// <param name="lePublic">lePublic</param>
        /// <param name="idRayon">idRayon</param>
        /// <param name="rayon">rayon</param>
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }

    }
}
