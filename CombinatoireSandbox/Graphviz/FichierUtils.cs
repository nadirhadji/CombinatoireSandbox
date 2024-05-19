namespace CombinatoireSandbox.Graphviz
{
    public static class FichierUtils
    {
        public static void SupprimerFichier(string chemin)
        {
            if (File.Exists(chemin))
            {
                File.Delete(chemin);
            }
        }

        public static bool EstRepertoireExistant(string cheminVersRepertorie)
        {
            return Directory.Exists(cheminVersRepertorie);
        }

        public static bool EstFichierExistant(string cheminVersFichier)
        {
            return File.Exists(cheminVersFichier);
        }

        public static string GenererNomFichier(string repertoire, string nomFichier)
        {
            if (!Directory.Exists(repertoire))
            {
                Directory.CreateDirectory(repertoire);
            }

            string extension = ".png";

            string fileName = $"{nomFichier}{extension}";
            string fullPath = Path.Combine(repertoire, fileName);
            return fullPath;
        }
    }
}
