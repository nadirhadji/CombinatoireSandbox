using System.Diagnostics;

namespace CombinatoireSandbox.Graphviz
{
    public static class GraphvizUtils
    {
        public static void ImprimerImageGraphviz(string contenuDot, string cheminImageSortie, bool forcerRecreation = false)
        {
            if (forcerRecreation && FichierUtils.EstFichierExistant(cheminImageSortie))
            {
                FichierUtils.SupprimerFichier(cheminImageSortie);
            }

            if (!FichierUtils.EstFichierExistant(cheminImageSortie))
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
                if (!string.IsNullOrEmpty(output))
                {
                    Console.WriteLine("Output:");
                    Console.WriteLine(output);
                }

                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
                }

                if (string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine($"Impression complété pour : {cheminImageSortie} \n");
                }
            }

            // Suppression du fichier temporaire
            File.Delete(fichierTempDot);
        }

        public static string GenererNomFichierPourPoset(string cheminBase, int n, int k)
        {
            string repertoireBase = $"{cheminBase}\\PrunningGrafting-K-{k}";
            string nomFichier = $"Poset-{n}-Noeud-{k}-Aretes";
            return FichierUtils.GenererNomFichier(repertoireBase, nomFichier);
        }

        public static string GenererNomFichierPourArbre(string cheminBase, int n, int k, string parenthesageArbre)
        {
            string repertoireBase = $"{cheminBase}\\Arbres-N-{n}-K-{k}";
            return FichierUtils.GenererNomFichier(repertoireBase, parenthesageArbre);
        }
    }
}
