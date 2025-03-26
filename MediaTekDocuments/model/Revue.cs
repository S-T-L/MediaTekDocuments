
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// périodicité de la  revue
        /// </summary>
        public string Periodicite { get; set; }
        /// <summary>
        /// délai de mise à disposition
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Valorise les propriétés
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="titre">titre</param>
        /// <param name="image">image</param>
        /// <param name="idGenre">idGenre</param>
        /// <param name="genre">genre</param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic">lePublic</param>
        /// <param name="idRayon">idRayon</param>
        /// <param name="rayon">rayon</param>
        /// <param name="periodicite">periodicite</param>
        /// <param name="delaiMiseADispo">delaiMiseADispo</param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            DelaiMiseADispo = delaiMiseADispo;
        }

    }
}
