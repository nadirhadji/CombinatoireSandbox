using CombinatoireSandbox.Arbre.ArbreGeneraux;
using static CombinatoireSandbox.Arbre.ArbreUtils;

namespace CombinatoireSandbox.PrunningGrafting.PrunningGraftingK
{
    public class PrunningGraftingK
    {
        private readonly string repertoirePosets;
        private readonly string repertoireArbres;

        public PrunningGraftingK(string repertoirePosets, string repertoireArbres)
        {
            this.repertoirePosets = repertoirePosets;
            this.repertoireArbres = repertoireArbres;
        }

        public string GenererPoset(int n, int k)
        {
            var toutLesArbres = GenerateurArbreK.GenererTousLesArbres(n, k);
            var mapDesSucesseurs = ObtenirMapDesSucceseurs(toutLesArbres, k);

            var graphvizService = new PrunningGraftingKGraphviz(repertoirePosets, repertoireArbres);
            var nomFichier = graphvizService.GenererVisualisationPruningGraftingK(mapDesSucesseurs, toutLesArbres, n, k);
            //var isLattice = IsLattice(mapDesSucesseurs, k);
            //Console.WriteLine($"Poset Coupe Greffe n = {n} ; k = {k} est un trelli ? {isLattice}");
            return nomFichier;
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> ObtenirMapDesSucceseurs(List<ElementArbreK> toutLesArbres, int k)
        {
            var mapDesSucceseursVide = Initialiser(toutLesArbres);
            var mapDesSucceseurs = ConstruireDictionnaireDesSucceseurs(toutLesArbres, mapDesSucceseursVide, k);
            return mapDesSucceseurs;
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> Initialiser(List<ElementArbreK> toutLesArbresDeTailleN)
        {
            var mapDesSucceseurs = new Dictionary<ElementArbreK, List<ElementArbreK>>();

            foreach (var arbre in toutLesArbresDeTailleN)
            {
                mapDesSucceseurs.TryAdd(arbre, new List<ElementArbreK>());
            }

            return mapDesSucceseurs;
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> ConstruireDictionnaireDesSucceseurs(List<ElementArbreK> toutLesArbresDeTailleN,
                                                                                                  Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucceseurs,
                                                                                                  int k)
        {
            // Definir tout les successeurs
            foreach (var arbre in toutLesArbresDeTailleN)
            {
                foreach (var successeur in Successors(arbre, k))
                {
                    mapDesSucceseurs[arbre].Add(successeur);
                }
            }

            return mapDesSucceseurs;
        }

        public List<ElementArbreK> Successors(ElementArbreK elementArbre, int k)
        {
            if (elementArbre is Feuille)
            {
                return new List<ElementArbreK>();
            }
            else if (elementArbre is Noeud)
            {
                var noeud = elementArbre as Noeud;
                var nombreEnfants = noeud.Enfants.Count;

                // Etape 1 - Plongée recursive ************************
                var combinee = new List<ElementArbreK>();

                for (int i = 0; i < nombreEnfants; i++)
                {
                    var enfant = noeud.Enfants[i];

                    var freresGauche = noeud.Enfants.Take(i).ToList();
                    var freresDroite = noeud.Enfants.Skip(i + 1).ToList();

                    var successeurs = Successors(enfant, k);

                    var nouveauxNoeudsRecursif = new List<ElementArbreK>();

                    for (int j = 0; j < successeurs.Count; j++)
                    {
                        var nouveauEnfants = new List<ElementArbreK>();
                        var sucesseur = successeurs[j];
                        nouveauEnfants.AddRange(freresGauche);
                        nouveauEnfants.Add(sucesseur);
                        nouveauEnfants.AddRange(freresDroite);
                        var noeudReconstruit = new Noeud(nouveauEnfants);
                        nouveauxNoeudsRecursif.Add(noeudReconstruit);
                    }

                    combinee.AddRange(nouveauxNoeudsRecursif);
                }

                // Etape 2 - Generation des noeuds dont le premier noeud enfant est supprimee 
                var arbresAvecPremierNoeudSuprimee = new List<Option<ElementArbreK>>();

                for (int l = 1; l < nombreEnfants; l++)
                {
                    var nouveauNoeudApresSupression = DeleteFirstNode(noeud.Enfants[l], k);
                    arbresAvecPremierNoeudSuprimee.Add(nouveauNoeudApresSupression);
                }

                // Etape 3 - Transfert des noeuds
                for (int m = 0; m < arbresAvecPremierNoeudSuprimee.Count; m++)
                {
                    var arbreAvecPremierNoeudSuprimee = arbresAvecPremierNoeudSuprimee[m];

                    if (arbreAvecPremierNoeudSuprimee.HasValue == true)
                    {
                        var ancienGauche = noeud.Enfants.Take(m).ToList();
                        var ancienDroite = noeud.Enfants.Skip(m + 2).ToList();

                        var arbreAvecDerniereFeuilleRemplacee = FillLastLeaf(noeud.Enfants[m], k);

                        var enfantNoeudCoupeeEtGreffee = new List<ElementArbreK>();
                        enfantNoeudCoupeeEtGreffee.AddRange(ancienGauche);
                        enfantNoeudCoupeeEtGreffee.Add(arbreAvecDerniereFeuilleRemplacee);
                        enfantNoeudCoupeeEtGreffee.Add(arbreAvecPremierNoeudSuprimee.Value);
                        enfantNoeudCoupeeEtGreffee.AddRange(ancienDroite);
                        var arbreApresCoupeEtGreffe = new Noeud(enfantNoeudCoupeeEtGreffee);

                        combinee.Insert(0, arbreApresCoupeEtGreffe);
                    }
                }

                return combinee;
            }
            else
            {
                throw new InvalidOperationException("Oulala, c'est le temps de debugger!");
            }
        }

        public ElementArbreK FillLastLeaf(ElementArbreK element, int k)
        {
            if (element is Feuille)
            {
                var feuilles = Enumerable.Repeat(new Feuille() as ElementArbreK, k).ToList();
                return new Noeud(feuilles);
            }
            else
            {
                var noeud = element as Noeud;
                var nouveauNoeud = new Noeud(k);

                var premiersNoeud = noeud.Enfants.GetRange(0, noeud.Enfants.Count - 1);
                var dernierNoeud = noeud.Enfants.Last();

                nouveauNoeud.Enfants.AddRange(premiersNoeud);
                var filled = FillLastLeaf(dernierNoeud, k);
                nouveauNoeud.Enfants.Add(filled);

                return nouveauNoeud;
            }
        }

        public Option<ElementArbreK> DeleteFirstNode(ElementArbreK tree, int k)
        {
            if (tree is Feuille)
            {
                return Option<ElementArbreK>.None;
            }
            else if (tree is Noeud node)
            {
                if (node.Enfants.All(n => n is Feuille))
                {
                    return Option<ElementArbreK>.Some(new Feuille());
                }
                else
                {
                    var premierNoeud = node.Enfants.First();
                    Option<ElementArbreK> nouveauPremier = DeleteFirstNode(premierNoeud, k);

                    if (nouveauPremier.HasValue)
                    {
                        var nouveauNoeud = new Noeud(k);
                        nouveauNoeud.Enfants.Add(nouveauPremier.Value);

                        var resteNoeud = node.Enfants.GetRange(1, node.Enfants.Count - 1);
                        nouveauNoeud.Enfants.AddRange(resteNoeud);

                        return Option<ElementArbreK>.Some(nouveauNoeud);
                    }
                    else
                    {
                        return Option<ElementArbreK>.None;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Erreur : type de l'element ne correspond pas a ElementArbreK");
            }
        }

        public bool IsCoveringRelation(ElementArbreK t, ElementArbreK t1, int k)
        {
            return Successors(t, k).Contains(t1);
        }

        public bool IsLeq(ElementArbreK t, ElementArbreK t1, int k)
        {
            if (Equals(t, t1)) return true;
            return Successors(t, k).Any(t2 => IsLeq(t2 as Noeud, t1 as Noeud, k));
        }

        public bool IsLattice(Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucesseurs, int k)
        {
            var elements = mapDesSucesseurs.Keys.ToList();

            foreach (var a in elements)
            {
                foreach (var b in elements)
                {
                    if (!ExistsJoin(elements, a, b, k) || !ExistsMeet(elements, a, b, k))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ExistsJoin(List<ElementArbreK> elements, ElementArbreK a, ElementArbreK b, int k)
        {
            var upperBounds = elements.Where(x => IsLeq(a, x, k) && IsLeq(b, x, k)).ToList();
            if (upperBounds.Count == 0) return false;

            var leastUpperBound = upperBounds.FirstOrDefault(ub => upperBounds.All(x => !IsLeq(x, ub, k)
                                                                || EqualityComparer<ElementArbreK>.Default.Equals(x, ub)));
            return leastUpperBound != null;
        }

        private bool ExistsMeet(List<ElementArbreK> elements, ElementArbreK a, ElementArbreK b, int k)
        {
            var lowerBounds = elements.Where(x => IsLeq(x, a, k) && IsLeq(x, b, k)).ToList();
            if (lowerBounds.Count == 0) return false;

            var greatestLowerBound = lowerBounds.FirstOrDefault(lb => lowerBounds.All(x => !IsLeq(lb, x, k)
                                                                                        || EqualityComparer<ElementArbreK>.Default.Equals(x, lb)));
            return greatestLowerBound != null;
        }
    }
}
