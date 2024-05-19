using CombinatoireSandbox.Graphviz;
using System.Text;

namespace CombinatoireSandbox.Arbre.ArbreGeneraux
{
    public class ArbreKGraphvizService
    {
        private int compteur;
        private StringBuilder noeuds;
        private StringBuilder aretes;

        public ArbreKGraphvizService()
        {
            compteur = 0;
            noeuds = new StringBuilder();
            aretes = new StringBuilder();
        }

        public string GenererImageArbreK(ElementArbreK arbre, string parenthesageLettre, int n, int k, string repertoireArbres)
        {
            var scriptGraphviz = GenererGraphviz(arbre);
            var cheminFichier = GraphvizUtils.GenererNomFichierPourArbre(repertoireArbres, n, k, parenthesageLettre);
            GraphvizUtils.ImprimerImageGraphviz(scriptGraphviz, cheminFichier);
            compteur = 0;
            noeuds.Clear();
            aretes.Clear();
            return cheminFichier;
        }

        public string GenererGraphviz(ElementArbreK racine)
        {
            StringBuilder graphviz = new StringBuilder();
            graphviz.AppendLine("graph ArbreK {");
            graphviz.AppendLine("    rankdir=TB;");
            graphviz.AppendLine("    nodesep = 0.1;");
            graphviz.AppendLine("    ranksep = 0.1;");
            graphviz.AppendLine("    node [shape=none, width=1, height=1];");

            ConstruireGraphe(racine);

            graphviz.Append(noeuds);
            graphviz.Append(aretes);

            graphviz.AppendLine("}");

            return graphviz.ToString();
        }

        public void ConstruireGraphe(ElementArbreK element, string parent = null)
        {
            if (element == null) return; // Condition d'arrêt pour les branches vides

            var currentId = element is Feuille ? $"F{++compteur}" : $"N{++compteur}";

            if (parent != null) // S'il y a un parent, ajoutez une arête entre le parent et l'élément actuel
            {
                noeuds.AppendLine($"    {currentId} [shape=circle, style=filled, color=palegreen, label=\"\", width=0.2, height=0.2];");
                aretes.AppendLine($"    {parent} -- {currentId} [len=0.1];");
            }

            if (element is Noeud noeud)
            {
                noeuds.AppendLine($"    {currentId} [shape=circle, style=filled, color=palegreen, label=\"\", width=0.2, height=0.2];");

                foreach (var enfants in noeud.Enfants)
                {
                    ConstruireGraphe(enfants, currentId);
                }
            }
            else if (element is Feuille)
            {
                // Traitement pour les feuilles
                noeuds.AppendLine($"    {currentId} [shape=square, style=filled, color=powderblue, label=\"\", width=0.1, height=0.1];");
            }
        }
    }
}