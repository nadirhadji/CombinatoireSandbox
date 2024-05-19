using CombinatoireSandbox.Arbre.ArbreBinaire;
using static CombinatoireSandbox.Arbre.ArbreUtils;

namespace CombinatoireSandbox.PrunningGrafting.PrunningGraftingBinaire
{
    public class PrunningGraftingBinaire
    {
        private readonly string repertoirePosets;
        private readonly string repertoireArbres;

        public PrunningGraftingBinaire(string repertoirePosets, string repertoireArbres)
        {
            this.repertoirePosets = repertoirePosets;
            this.repertoireArbres = repertoireArbres;
        }

        public string GenererPoset(int nombreNoeud)
        {
            var graphvizService = new PrunningGraftingBinaireGraphviz(repertoirePosets, repertoireArbres);
            var toutLesArbres = GenerateurArbreBinaire.GenererToutLesArbres(nombreNoeud);
            var mapDesSucesseurs = ObtenirMapDesSucceseurs(toutLesArbres);
            return graphvizService.GenererVisualisationPruningGrafting(mapDesSucesseurs, toutLesArbres, nombreNoeud);
        }

        private Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> ObtenirMapDesSucceseurs(List<ElementArbreBinaire> toutLesArbres)
        {
            var mapDesSucceseursVide = Initialiser(toutLesArbres);
            var mapDesSucceseurs = ConstruireDictionnaireDesSucceseurs(toutLesArbres, mapDesSucceseursVide);
            return mapDesSucceseurs;
        }

        private Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> Initialiser(List<ElementArbreBinaire> toutLesArbresDeTailleN)
        {
            var mapDesSucceseurs = new Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>>();

            foreach (var arbre in toutLesArbresDeTailleN)
            {
                mapDesSucceseurs.TryAdd(arbre, new List<ElementArbreBinaire>());
            }

            return mapDesSucceseurs;
        }

        private Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> ConstruireDictionnaireDesSucceseurs(List<ElementArbreBinaire> toutLesArbresDeTailleN, Dictionary<ElementArbreBinaire, List<ElementArbreBinaire>> mapDesSucceseurs)
        {
            // Definir tout les successeurs
            foreach (var arbre in toutLesArbresDeTailleN)
            {
                foreach (var successeur in Successors(arbre))
                {
                    mapDesSucceseurs[arbre].Add(successeur);
                }
            }

            return mapDesSucceseurs;
        }

        private List<ElementArbreBinaire> Successors(ElementArbreBinaire t)
        {
            switch (t)
            {
                case Feuille:
                    return new List<ElementArbreBinaire>();

                case Noeud n when n.Gauche is ElementArbreBinaire && n.Droite is Feuille:
                    return Successors(n.Gauche).Select(t1 => new Noeud(t1, new Feuille()) as ElementArbreBinaire).ToList();

                case Noeud n when n.Gauche is ElementArbreBinaire && n.Droite is ElementArbreBinaire:

                    var leftSuccessors = Successors(n.Gauche).Select(t1 => new Noeud(t1, n.Droite) as ElementArbreBinaire).ToList();
                    var rightSuccessors = Successors(n.Droite).Select(t2 => new Noeud(n.Gauche, t2) as ElementArbreBinaire).ToList();
                    var combined = leftSuccessors.Concat(rightSuccessors).ToList();

                    Option<ElementArbreBinaire> nouveauDroite = DeleteFirstNode(n.Droite);

                    if (nouveauDroite.HasValue == false)
                    {
                        return combined;
                    }
                    else
                    {
                        ElementArbreBinaire filledLastLeaf = FillLastLeaf(n.Gauche);
                        combined.Insert(0, new Noeud(filledLastLeaf, nouveauDroite.Value));
                    }

                    return combined;

                default:
                    return null;
            }
        }

        private ElementArbreBinaire FillLastLeaf(ElementArbreBinaire t)
        {
            switch (t)
            {
                case Feuille:
                    return new Noeud(new Feuille(), new Feuille());

                case Noeud n when n.Gauche is ElementArbreBinaire && n.Droite is ElementArbreBinaire:
                    return new Noeud(n.Gauche, FillLastLeaf(n.Droite));

                default:
                    return null; // Should not occur
            }
        }

        private Option<ElementArbreBinaire> DeleteFirstNode(ElementArbreBinaire tree)
        {
            if (tree is Feuille)
            {
                return Option<ElementArbreBinaire>.None;
            }
            else if (tree is Noeud node)
            {
                if (node.Gauche is Feuille && node.Droite is Feuille)
                {
                    return Option<ElementArbreBinaire>.Some(new Feuille());
                }
                else
                {
                    Option<ElementArbreBinaire> nouveauGauche = DeleteFirstNode(node.Gauche);
                    if (nouveauGauche.HasValue)
                    {
                        return Option<ElementArbreBinaire>.Some(new Noeud(nouveauGauche.Value, node.Droite));
                    }
                    else
                    {
                        return Option<ElementArbreBinaire>.None;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Erreur Innatendu : type de l'element ne correspond pas a ElementArbreBinaire");
            }
        }

        private bool IsCoveringRelation(ElementArbreBinaire t, ElementArbreBinaire t1)
        {
            return Successors(t).Contains(t1);
        }

        private bool IsLeq(ElementArbreBinaire t, ElementArbreBinaire t1)
        {
            if (Equals(t, t1)) return true;
            return Successors(t).Any(t2 => IsLeq(t2 as Noeud, t1 as Noeud));
        }
    }
}
