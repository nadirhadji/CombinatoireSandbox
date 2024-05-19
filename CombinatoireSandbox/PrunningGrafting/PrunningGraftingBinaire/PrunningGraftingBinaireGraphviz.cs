using CombinatoireSandbox.Arbre.ArbreBinaire;
using CombinatoireSandbox.Graphviz;
using System.Text;

namespace CombinatoireSandbox.PrunningGrafting.PrunningGraftingBinaire
{
    public class PrunningGraftingBinaireGraphviz
    {
        private readonly string repertoirePosets;
        private readonly string repertoireArbres;

        public PrunningGraftingBinaireGraphviz(string repertoirePosets, string repertoireArbres)
        {
            this.repertoirePosets = repertoirePosets;
            this.repertoireArbres = repertoireArbres;
        }

        public string GenererVisualisationPruningGrafting(Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> mapDesSucceseurs,
                                                        List<ElementArbreBinaire> toutLesArbres, int nombreNoeud)
        {
            var scriptGraphviz = GenererPosetEnGraphviz(mapDesSucceseurs, toutLesArbres, repertoireArbres, nombreNoeud);
            var cheminImagePosets = GraphvizUtils.GenererNomFichierPourPoset(repertoirePosets, nombreNoeud, 2);
            GraphvizUtils.ImprimerImageGraphviz(scriptGraphviz, cheminImagePosets);
            return cheminImagePosets;
        }

        private string GenererPosetEnGraphviz(Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> mapDesSucceseurs,
                                              List<ElementArbreBinaire> toutLesArbres, string repertoireArbres, int nombreNoeud)
        {
            var dot = new StringBuilder();

            dot.AppendLine("graph Poset { ");
            dot.AppendLine("rankdir=TB; ");
            dot.AppendLine($"node [shape=none, width=3, height=1.2]; ");

            foreach (var arbre in toutLesArbres)
            {
                var graphvizArbreBinaireService = new ArbreBinaireGraphviz();
                var parenthesageArbre = arbre.ObtenirParenthesageLettres();
                var cheminVersImage = graphvizArbreBinaireService.GenererImageArbreBinaire(arbre, parenthesageArbre, nombreNoeud, repertoireArbres);

                dot.AppendLine($"{parenthesageArbre} [label=\"\" image = \"{cheminVersImage}\"]; ");
            }

            var relationOrdre = DefinirRelationOrdreEnGraphviz(mapDesSucceseurs);

            dot.Append(relationOrdre);

            //Accolade fermente script dot
            dot.AppendLine("}");

            return dot.ToString();
        }

        private string DefinirRelationOrdreEnGraphviz(Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> mapDesSucceseurs)
        {
            var dot = new StringBuilder();

            foreach (var relationOrdre in mapDesSucceseurs)
            {
                var parenthesageArbre = relationOrdre.Key.ObtenirParenthesageLettres();

                foreach (var successeur in relationOrdre.Value)
                {
                    var parenthesageSucesseur = successeur.ObtenirParenthesageLettres();
                    dot.AppendLine($"{parenthesageArbre} -- {parenthesageSucesseur} [penwidth=1]; ");
                }
            }

            return dot.ToString();
        }
    }
}
