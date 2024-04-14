using CombinatoireSandbox.ArbreBinaire;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using System.Diagnostics;

public static class ArbreBinaireExtension
{
    private static int id = 0;

    private static void ConstruireGraphe(ElementArbre element, AdjacencyGraph<string, Edge<string>> graph, string parent = null)
    {
        if (element == null) return; // Condition d'arrêt pour les branches vides

        var currentId = element is Feuille ? $"F{++id}" : $"N{++id}";

        if (parent != null) // S'il y a un parent, ajoutez une arête entre le parent et l'élément actuel
        {
            graph.AddVerticesAndEdge(new Edge<string>(parent, currentId));
        }

        if (element is Noeud noeud)
        {
            // Traitement pour les nœuds
            // Ajouter le nœud actuel au graphe (si pas déjà fait par AddVerticesAndEdge)
            graph.AddVertex(currentId);

            // Appels récursifs pour les enfants gauche et droit
            ConstruireGraphe(noeud.Gauche, graph, currentId);
            ConstruireGraphe(noeud.Droite, graph, currentId);
        }
        else if (element is Feuille)
        {
            // Traitement pour les feuilles
            graph.AddVertex(currentId);
        }
    }

    public static string GenererGraphviz(ElementArbre arbre)
    {
        // Convertir l'arbre binaire en graphe QuikGraph
        var graph = new AdjacencyGraph<string, Edge<string>>();
        ConstruireGraphe(arbre, graph);

        // Générer la représentation Graphviz de l'arbre
        var graphviz = new GraphvizAlgorithm<string, Edge<string>>(graph);

        // Configurer le formatage des sommets
        graphviz.FormatVertex += (sender, args) =>
        {
            if (args.Vertex.StartsWith("F")) // Identifier les feuilles par leur ID
            {
                // Attribuer la forme de boîte aux feuilles
                args.VertexFormat.Shape = GraphvizVertexShape.Box;
                args.VertexFormat.Style = GraphvizVertexStyle.Filled;
                args.VertexFormat.FillColor = GraphvizColor.PowderBlue;
            }
            else
            {
                // Attribuer une autre forme ou des attributs par défaut aux nœuds
                args.VertexFormat.Shape = GraphvizVertexShape.Ellipse;
                args.VertexFormat.Style = GraphvizVertexStyle.Filled;
                args.VertexFormat.FillColor = GraphvizColor.PaleGreen;
            }
        };

        // Générer le text graphviz
        var graphvizText = graphviz.Generate();

        //Console.WriteLine(graphvizText);

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
        string baseDir = $"C:\\Users\\nadir\\Data\\Graphviz\\ArbreBinaire\\Taille-{tailleArbreBinaire}\\"; // Répertoire de base

        if (!Directory.Exists(baseDir))
        {
            Directory.CreateDirectory(baseDir);
        }

        string baseName = $"ArbreBinaire-Taille4-"; // Nom de base du fichier
        string dateTimeFormat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"); // Format de date/heure
        string extension = ".png"; // Extension du fichier

        string fileName = $"{baseName}_{dateTimeFormat}{extension}"; // Construit le nom du fichier
        string fullPath = Path.Combine(baseDir, fileName); // Construit le chemin complet
        return fullPath;
    }

    public static void Executer(int nombreNoeudInterne)
    {
        var toutLesArbres = GenererToutLesArbres(nombreNoeudInterne);

        foreach (var arbre in toutLesArbres)
        {
            Console.WriteLine(arbre.Afficher());
            var graphzivText = GenererGraphviz(arbre);
            var nomFichier = GenererNomFichierArbreBinaire(nombreNoeudInterne);
            GenererImageGraphviz(graphzivText, nomFichier);
        }
    }

    public static IEnumerable<ElementArbre> GenererToutLesArbres(int n)
    {
        if (n == 0)
        {
            return new List<ElementArbre>() { new Feuille() };
        }

        var resultat = new List<ElementArbre>();

        for (var n1 = 0; n1 <= (n - 1); n1++)
        {
            foreach (var t1 in GenererToutLesArbres(n1))
            {
                foreach (var t2 in GenererToutLesArbres((n - 1) - n1))
                {
                    var t = new Noeud(t1, t2);
                    resultat.Insert(0, t);
                }
            }
        }

        return resultat;
    }
}