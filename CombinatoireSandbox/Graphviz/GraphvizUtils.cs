using System.Diagnostics;

namespace CombinatoireSandbox.Graphviz
{
    public static class GraphvizUtils
    {
        public static void ImprimerImageGraphviz(string contenuDot, string cheminImageSortie, bool forcerRecreation = false)
        {
            if (forcerRecreation && EstFichierExistant(cheminImageSortie))
            {
                SupprimerFichier(cheminImageSortie);
            }

            if (!EstFichierExistant(cheminImageSortie))
            {
                GenererImageGraphviz(contenuDot, cheminImageSortie);
            }
        }

        public static void GenererImageGraphviz(string contenuDot, string cheminImageSortie)
        {
            string cheminDotExe = "C:\\Program Files\\Graphviz\\bin\\dot.exe";

            // Création d'un fichier temporaire pour stocker le contenu DOT
            string fichierTempDot = Path.GetTempFileName();
            File.WriteAllText(fichierTempDot, contenuDot);

            // Configuration du processus pour exécuter Graphviz
            ProcessStartInfo startInfo = new ProcessStartInfo(cheminDotExe)
            {
                Arguments = $"-Tpng \"{fichierTempDot}\" -o \"{cheminImageSortie}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // Exécution de Graphviz pour générer l'image
            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Affichage des erreurs et de la sortie standard pour le débogage
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                Console.WriteLine("Errors:");
                Console.WriteLine(errors);
            }

            // Suppression du fichier temporaire
            File.Delete(fichierTempDot);
        }

        public static void SupprimerFichier(string chemin)
        {
            if (File.Exists(chemin))
            {
                File.Delete(chemin);
            }
        }

        public static bool EstFichierExistant(string cheminVersFichier)
        {
            return File.Exists(cheminVersFichier);
        }

        public static string GenererNomFichierPourPoset(string cheminBase, int n, int k)
        {
            string repertoireBase = $"{cheminBase}\\PrunningGrafting-K-{k}";
            string nomFichier = $"Poset-{n}-Noeud-{k}-Aretes";
            return GenererNomFichier(repertoireBase, nomFichier);
        }

        public static string GenererNomFichierPourArbre(string cheminBase, int n, int k, string parenthesageArbre)
        {
            string repertoireBase = $"{cheminBase}\\Arbres-N-{n}-K-{k}";
            return GenererNomFichier(repertoireBase, parenthesageArbre);
        }

        private static string GenererNomFichier(string repertoire, string nomFichier)
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
