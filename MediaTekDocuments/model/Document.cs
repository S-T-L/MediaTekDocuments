
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Titre
        /// </summary>
        public string Titre { get; }
        /// <summary>
        /// image
        /// </summary>
        public string Image { get; }
        /// <summary>
        /// id du genre
        /// </summary>
        public string IdGenre { get; }
        /// <summary>
        /// genre
        /// </summary>
        public string Genre { get; }
        /// <summary>
        /// id du public
        /// </summary>
        public string IdPublic { get; }
        /// <summary>
        /// public
        /// </summary>
        public string Public { get; }
        /// <summary>
        /// id du rayon
        /// </summary>
        public string IdRayon { get; }
        /// <summary>
        /// rayon
        /// </summary>
        public string Rayon { get; }

        /// <summary>
        /// Valorise les propriétés
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
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
