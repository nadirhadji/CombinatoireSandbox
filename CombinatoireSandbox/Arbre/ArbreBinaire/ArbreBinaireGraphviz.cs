using CombinatoireSandbox.Graphviz;
using System.Text;

namespace CombinatoireSandbox.Arbre.ArbreBinaire
{
    public class ArbreBinaireGraphviz
    {
        private int compteur;
        private StringBuilder noeuds;
        private StringBuilder aretes;

        public ArbreBinaireGraphviz()
        {
            compteur = 0;
            noeuds = new StringBuilder();
            aretes = new StringBuilder();
        }

        public string GenererImagesToutLesArbreBinaire(int n, string repertoireArbres)
        {
            var arbres = GenerateurArbreBinaire.GenererToutLesArbres(n);

            foreach (var arbre in arbres)
            {
                var parenthesageLettre = arbre.ObtenirParenthesageLettres();
                GenererImageArbreBinaire(arbre, parenthesageLettre, n, repertoireArbres);
            }

            return repertoireArbres;
        }

        public string GenererImageArbreBinaire(ElementArbreBinaire arbre, string parenthesageLettre, int nombreNoeudInterne, string repertoireArbres)
        {
            var scriptGraphviz = GenererGraphviz(arbre);
            var nomFichier = GraphvizUtils.GenererNomFichierPourArbre(repertoireArbres, nombreNoeudInterne, 2, parenthesageLettre);
            GraphvizUtils.ImprimerImageGraphviz(scriptGraphviz, nomFichier);
            compteur = 0;
            noeuds.Clear();
            aretes.Clear();
            return nomFichier;
        }

        public string GenererGraphviz(ElementArbreBinaire racine)
        {
            StringBuilder graphviz = new StringBuilder();
            graphviz.AppendLine("graph Arbre {");
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

        public void ConstruireGraphe(ElementArbreBinaire element, string parent = null)
        {
            if (element == null) return; // Condition d'arr�t pour les branches vides

            var currentId = element is Feuille ? $"F{++compteur}" : $"N{++compteur}";

            if (parent != null) // S'il y a un parent, ajoutez une ar�te entre le parent et l'�l�ment actuel
            {
                noeuds.AppendLine($"    {currentId} [shape=circle, style=filled, color=palegreen, label=\"\", width=0.2, height=0.2];");
                aretes.AppendLine($"    {parent} -- {currentId} [len=0.1];");
            }

            if (element is Noeud noeud)
            {
                noeuds.AppendLine($"    {currentId} [shape=circle, style=filled, color=palegreen, label=\"\", width=0.2, height=0.2];");

                ConstruireGraphe(noeud.Gauche, currentId);
                ConstruireGraphe(noeud.Droite, currentId);
            }
            else if (element is Feuille)
            {
                // Traitement pour les feuilles
                noeuds.AppendLine($"    {currentId} [shape=square, style=filled, color=powderblue, label=\"\", width=0.1, height=0.1];");
            }
        }
    }
}