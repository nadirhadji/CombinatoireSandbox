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
                var parenthesageArbre = arbre.ObtenirParenthesage();
                var parenthesageLettre = ConvertirParenthesageEnLettre(parenthesageArbre);
                var cheminVersImage = graphvizArbreBinaireService.GenererImageArbreBinaire(arbre, parenthesageLettre, nombreNoeud, repertoireArbres);

                dot.AppendLine($"{parenthesageLettre} [label=\"\" image = \"{cheminVersImage}\"]; ");
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
                var parenthesageArbre = relationOrdre.Key.ObtenirParenthesage();
                var nomNoeudGraphvizArbre = ConvertirParenthesageEnLettre(parenthesageArbre);

                foreach (var successeur in relationOrdre.Value)
                {
                    var parenthesageSucesseur = successeur.ObtenirParenthesage();
                    var nomNoeudGraphvizSucceseur = ConvertirParenthesageEnLettre(parenthesageSucesseur);

                    dot.AppendLine($"{nomNoeudGraphvizArbre} -- {nomNoeudGraphvizSucceseur} [penwidth=1]; ");
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
