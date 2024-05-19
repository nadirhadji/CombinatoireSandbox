using CombinatoireSandbox.Arbre.ArbreGeneraux;
using CombinatoireSandbox.Graphviz;
using System.Text;

namespace CombinatoireSandbox.PrunningGrafting.PrunningGraftingK
{
    public class PrunningGraftingKGraphviz
    {
        private readonly string repertoirePosets;
        private readonly string repertoireArbres;

        public PrunningGraftingKGraphviz(string repertoirePosets, string repertoireArbres)
        {
            this.repertoirePosets = repertoirePosets;
            this.repertoireArbres = repertoireArbres;
        }

        public string GenererVisualisationPruningGraftingK(Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucceseurs,
                                                         List<ElementArbreK> toutLesArbres, int n, int k)
        {
            var scriptGraphviz = GenererPosetEnGraphviz(mapDesSucceseurs, toutLesArbres, repertoireArbres, n, k);
            var cheminImagePosets = GraphvizUtils.GenererNomFichierPourPoset(repertoirePosets, n, k);
            GraphvizUtils.ImprimerImageGraphviz(scriptGraphviz, cheminImagePosets);
            return cheminImagePosets;
        }

        private string GenererPosetEnGraphviz(Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucceseurs,
                                         List<ElementArbreK> toutLesArbres, string repertoireArbres, int n, int k)
        {
            var dot = new StringBuilder();

            dot.AppendLine("graph PosetK { ");
            dot.AppendLine("rankdir=TB; ");
            dot.AppendLine($"node [shape=none, width=3, height=1.2]; ");

            foreach (var arbre in toutLesArbres)
            {
                var graphvizArbreKService = new ArbreKGraphviz();
                var parenthesageArbre = arbre.ObtenirParenthesageLettres();
                var cheminVersImage = graphvizArbreKService.GenererImageArbreK(arbre, parenthesageArbre, n, k, repertoireArbres);

                dot.AppendLine($"{parenthesageArbre} [label=\"\" image = \"{cheminVersImage}\"]; ");
            }

            var relationOrdre = DefinirRelationOrdreEnGraphviz(mapDesSucceseurs);

            dot.Append(relationOrdre);

            //Accolade fermente script dot
            dot.AppendLine("}");

            return dot.ToString();
        }

        private string DefinirRelationOrdreEnGraphviz(Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucceseurs)
        {
            var dot = new StringBuilder();

            foreach (var relationOrdre in mapDesSucceseurs)
            {
                var parenthesageArbre = relationOrdre.Key.ObtenirParenthesageLettres();

                foreach (var successeur in relationOrdre.Value)
                {
                    var parenthesageSucesseur = successeur.ObtenirParenthesageLettres();
                    dot.AppendLine($"{parenthesageArbre} -- {parenthesageSucesseur} [penwidth=1.0]; ");
                }
            }

            return dot.ToString();
        }
    }
}
