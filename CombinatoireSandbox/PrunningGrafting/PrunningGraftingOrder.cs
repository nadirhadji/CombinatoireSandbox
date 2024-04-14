using CombinatoireSandbox.ArbreBinaire;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using System.Diagnostics;

namespace CombinatoireSandbox.PrunningGrafting
{
    public class RecursivePrunningGraftingOrder
    {
        public struct Option<T>
        {
            public static Option<T> None { get; } = new Option<T>();

            public T Value { get; }
            public bool HasValue { get; }

            private Option(T value)
            {
                Value = value;
                HasValue = true;
            }

            public static Option<T> Some(T value)
            {
                return new Option<T>(value);
            }
        }

        public static ElementArbre DeleteFirstNode(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    throw new Exception("Error.");

                case Noeud n when n.Gauche is Feuille && n.Droite is Feuille:
                    return new Feuille();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:
                    return new Noeud(DeleteFirstNode(n.Gauche), n.Droite);

                default:
                    return null; // Should not occur
            }
        }

        public static Option<ElementArbre> DeleteFirstNode2(ElementArbre tree)
        {
            if (tree is Feuille)
            {
                return Option<ElementArbre>.None;
            }
            else if (tree is Noeud node)
            {
                if (node.Gauche is Feuille && node.Droite is Feuille)
                {
                    return Option<ElementArbre>.Some(new Feuille());
                }
                else
                {
                    Option<ElementArbre> nouveauGauche = DeleteFirstNode2(node.Gauche);
                    if (nouveauGauche.HasValue)
                    {
                        return Option<ElementArbre>.Some(new Noeud(nouveauGauche.Value, node.Droite));
                    }
                    else
                    {
                        return Option<ElementArbre>.None;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Erreur Innatendu : type de l'element ne correspond pas a ElementArbre");
            }
        }

        public static ElementArbre FillLastLeaf(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    return new Noeud(new Feuille(), new Feuille());

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:
                    return new Noeud(n.Gauche, FillLastLeaf(n.Droite));

                default:
                    return null; // Should not occur
            }
        }

        public static List<ElementArbre> Successors(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    return new List<ElementArbre>();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is Feuille:
                    return Successors(n.Gauche).Select(t1 => new Noeud(t1, new Feuille()) as ElementArbre).ToList();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:

                    var leftSuccessors = Successors(n.Gauche).Select(t1 => new Noeud(t1, n.Droite) as ElementArbre).ToList();
                    var rightSuccessors = Successors(n.Droite).Select(t2 => new Noeud(n.Gauche, t2) as ElementArbre).ToList();
                    var combined = leftSuccessors.Concat(rightSuccessors).ToList();

                    Option<ElementArbre> nouveauDroite = DeleteFirstNode2(n.Droite);

                    if (nouveauDroite.HasValue == false)
                    {
                        return combined;
                    }
                    else
                    {
                        ElementArbre filledLastLeaf = FillLastLeaf(n.Gauche);
                        combined.Insert(0, new Noeud(filledLastLeaf, nouveauDroite.Value));
                    }

                    return combined;

                default:
                    return null;
            }
        }

        public static bool IsCoveringRelation(Noeud t, Noeud t1)
        {
            return Successors(t).Contains(t1); // This presumes Noeud implements an appropriate equality comparison
        }

        public static bool IsLeq(Noeud t, Noeud t1)
        {
            if (Equals(t, t1)) return true;
            return Successors(t).Any(t2 => IsLeq(t2 as Noeud, t1 as Noeud));
        }

        public static IEnumerable<ElementArbre> GenererToutLesArbres(int n)
        {
            if (n == 0)
            {
                return new List<ElementArbre>() { new Feuille() };
            }

            var resultat = new List<ElementArbre>();

            for (var n1 = 0; n1 <= n - 1; n1++)
            {
                foreach (var t1 in GenererToutLesArbres(n1))
                {
                    foreach (var t2 in GenererToutLesArbres(n - 1 - n1))
                    {
                        var t = new Noeud(t1, t2);
                        resultat.Insert(0, t);
                    }
                }
            }

            return resultat;
        }

        public static void GenererOrdreGraftingPrunning(int taille)
        {
            var arbres = GenererToutLesArbres(taille);

            foreach (var arbre in arbres)
            {
                var graphzivArbre = ArbreBinaireExtension.GenererGraphviz(arbre);

                Console.WriteLine("Arbre : ");
                Console.WriteLine(arbre.Afficher());
                Console.WriteLine("\n");

                var succuseurs = Successors(arbre);
                Console.WriteLine($"Sucesseurs : {succuseurs.Count()}");

                foreach (var succ in succuseurs)
                {
                    var graphzivSucc = ArbreBinaireExtension.GenererGraphviz(succ);

                    Console.WriteLine("Sucesseur : ");
                    Console.WriteLine(succ.Afficher());
                    Console.WriteLine("\n");
                }
            }
        }

        public static string GenererOrdreGraftingPrunningGraphviz(int taille)
        {
            var arbres = GenererToutLesArbres(taille).ToList();

            // Convertir the poset en a QuikGraph graph
            var graph = new AdjacencyGraph<string, Edge<string>>();

            for (int i = 0; i < arbres.Count; i++)
            {
                var currentId = $"T{i+1} [label=< {ArbreBinaireExtension.GenererGraphviz(arbres[i])} >]";

                graph.AddVertex(currentId);

                // Add an edge from each tree to its successors
                var successeurs = Successors(arbres[i]);
                foreach (var succ in successeurs)
                {
                    var succId = $"T{arbres.IndexOf(succ)+1} [label=< {ArbreBinaireExtension.GenererGraphviz(succ)} >]";
                    graph.AddVerticesAndEdge(new Edge<string>(currentId, succId));
                }
            }

            // Generate the Graphviz representation of the poset
            var graphviz = new GraphvizAlgorithm<string, Edge<string>>(graph);

            // Configure the vertex formatting
            graphviz.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Shape = GraphvizVertexShape.Unspecified;
                args.VertexFormat.Style = GraphvizVertexStyle.Unspecified;
                args.VertexFormat.FillColor = GraphvizColor.White;
            };

            // Generate the Graphviz text
            var graphvizText = graphviz.Generate();

            return graphvizText;
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

        private static string GenererNomFichierArbreBinaire(int tailleArbreBinaire)
        {
            string baseDir = $"C:\\Users\\nadir\\Data\\Graphviz\\PrunningGraftingOrder\\Taille-{tailleArbreBinaire}\\"; // Répertoire de base

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string baseName = $"PrunningGraftingOrder-Taille4-"; // Nom de base du fichier
            string dateTimeFormat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"); // Format de date/heure
            string extension = ".png"; // Extension du fichier

            string fileName = $"{baseName}_{dateTimeFormat}{extension}"; // Construit le nom du fichier
            string fullPath = Path.Combine(baseDir, fileName); // Construit le chemin complet
            return fullPath;
        }

        public static void Executer(int nombreNoeudInterne)
        {
            var graphzivText = GenererOrdreGraftingPrunningGraphviz(nombreNoeudInterne);
            var nomFichier = GenererNomFichierArbreBinaire(nombreNoeudInterne);
            GenererImageGraphviz(graphzivText, nomFichier);
        }
    }
}
