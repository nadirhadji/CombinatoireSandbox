using System.Diagnostics;

namespace CombinatoireSandbox.Graphviz
{
    public static class GraphvizUtils
    {
        public static void ImprimerImageGraphviz(string contenuDot, string cheminImageSortie)
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

        public static void OuvrirFichier(string chemin)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = chemin,
                UseShellExecute = true,
            });
        }

        public static string GenererNomFichierPourPoset(string cheminBase, int n, int k)
        {
            string repertoireBase = $"{cheminBase}\\Taille-{n}-{k}\\";
            string nomFichier = $"Posets-Pour-{n}-Noeud-{k}-Aretes";
            return GenererNomFichier(repertoireBase, nomFichier);
        }

        public static string GenererNomFichierPourArbre(string cheminBase, int n, int k)
        {
            string repertoireBase = $"{cheminBase}\\Taille-{n}-{k}\\";
            string nomFichier = $"Arbres-{n}-Noeud-{k}-Aretes";
            return GenererNomFichier(repertoireBase, nomFichier);
        }

        private static string GenererNomFichier(string repertoire, string nom)
        {
            if (!Directory.Exists(repertoire))
            {
                Directory.CreateDirectory(repertoire);
            }

            string dateTimeFormat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"); // Format de date/heure
            string extension = ".png"; // Extension du fichier

            string fileName = $"{nom}_{dateTimeFormat}{extension}"; // Construit le nom du fichier
            string fullPath = Path.Combine(repertoire, fileName); // Construit le chemin complet
            return fullPath;
        }
    }
}
