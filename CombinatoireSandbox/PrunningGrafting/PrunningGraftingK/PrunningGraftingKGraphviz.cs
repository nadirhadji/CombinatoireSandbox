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
                var parenthesageArbre = arbre.ObtenirParenthesage();
                var parenthesageLettre = ConvertirParenthesageEnLettre(parenthesageArbre);
                var cheminVersImage = graphvizArbreKService.GenererImageArbreK(arbre, parenthesageLettre, n, k, repertoireArbres);

                dot.AppendLine($"{parenthesageLettre} [label=\"\" image = \"{cheminVersImage}\"]; ");
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
                var parenthesageArbre = relationOrdre.Key.ObtenirParenthesage();
                var nomNoeudGraphvizArbre = ConvertirParenthesageEnLettre(parenthesageArbre);

                foreach (var successeur in relationOrdre.Value)
                {
                    var parenthesageSucesseur = successeur.ObtenirParenthesage();
                    var nomNoeudGraphvizSucceseur = ConvertirParenthesageEnLettre(parenthesageSucesseur);

                    dot.AppendLine($"{nomNoeudGraphvizArbre} -- {nomNoeudGraphvizSucceseur} [penwidth=1.0]; ");
                }
            }

            return dot.ToString();
        }

        //Les parenthese ne sont pas tolere pour les noms de nooeud dans graphviz
        private string ConvertirParenthesageEnLettre(string parenthesage)
        {
            return new string(parenthesage.Select(c => c == '(' ? 'N' : 'F').ToArray());
        }
    }
}
