
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd : LivreDvd
    {
       /// <summary>
       /// durée du dvd
       /// </summary>
        public int Duree { get; }
        /// <summary>
        /// Réalisateur du dvd
        /// </summary>
        public string Realisateur { get; }
        /// <summary>
        /// Synopsis
        /// </summary>
        public string Synopsis { get; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="titre">titre</param>
        /// <param name="image">image</param>
        /// <param name="duree">duree</param>
        /// <param name="realisateur">realisateur</param>
        /// <param name="synopsis">synopsis</param>
        /// <param name="idGenre">idGenre</param>
        /// <param name="genre">genre</param>
        /// <param name="idPublic">idPublic</param>
        /// <param name="lePublic">lePublic</param>
        /// <param name="idRayon">idRayon</param>
        /// <param name="rayon">rayon</param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Duree = duree;
            this.Realisateur = realisateur;
            this.Synopsis = synopsis;
        }

    }
}
