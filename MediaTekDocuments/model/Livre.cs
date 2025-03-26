
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres
    /// </summary>
    public class Livre : LivreDvd
    {
        /// <summary>
        /// Isbn 
        /// </summary>
        public string Isbn { get; }
        /// <summary>
        /// Auteur du livre
        /// </summary>
        public string Auteur { get; }
        /// <summary>
        /// Collection du livre
        /// </summary>
        public string Collection { get; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="titre">titre</param>
        /// <param name="image">image</param>
        /// <param name="isbn">isbn</param>
        /// <param name="auteur">auteur</param>
        /// <param name="collection">collection</param>
        /// <param name="idGenre">idGenre</param>
        /// <param name="genre">genre</param>
        /// <param name="idPublic">idPublic</param>
        /// <param name="lePublic">lePublic</param>
        /// <param name="idRayon">idRayon</param>
        /// <param name="rayon">rayon</param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Isbn = isbn;
            this.Auteur = auteur;
            this.Collection = collection;
        }



    }
}
